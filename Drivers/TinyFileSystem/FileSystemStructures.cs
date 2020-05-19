/*
 * TinyFileSystem driver for TinyCLR 2.0
 * 
 * Version 1.0
 *  - Initial revision, based on Chris Taylor (Taylorza) work
 *  - adaptations to conform to MikroBus.Net drivers design
 *  
 *  
 * Copyright 2020 MikroBus.Net
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */

using System;
using System.Text;

namespace MBN.Modules
{
  // The following summarizes the physical layout of the file system structures on the device.
  //
  // File Entry Cluster 
  // -------+-------+-----------------
  // offset | bytes | Description
  // -------+-------+-----------------  
  // 0      | 1     | Cluster Marker 
  //        |       |   1st Cluster of Sector : 0xff - unformatted/invalid sector, 0x7f - formatted sector/free cluster
  //        |       |   nth Cluster of Sector : 0xff - free cluster, 0x3f - allocated cluster
  //        |       |   0x1f - Orphaned page
  // 1      | 2     | Object ID > 0
  // 3      | 2     | Block Id  
  // 5      | 2     | Data Length
  // 7      | 1     | FileName Length
  // 8      | 16    | FileName
  // 24     | 8     | CreationTime
  // 32     | n     | The first n bytes of data contained in the file. The max value of n is dependent on the cluster size.

  // File Data Cluster
  // -------+-------+-----------------
  // offset | bytes | Description
  // -------+-------+-----------------  
  // 0      | 1     | Cluster Marker 
  //        |       |   1st Cluster of Sector : 0xff - unformatted/invalid sector, 0x7f - formatted sector/free cluster
  //        |       |   nth Cluster of Sector : 0xff - free cluster, 0x3f - allocated cluster
  //        |       |   0x1f - Orphaned page
  // 1      | 2     | Object ID = 0
  // 3      | 2     | Block Id  
  // 5      | 2     | Data Length
  // 7      | n     | n bytes of data contained in the file at Block Id. The max value of n is dependent on the cluster size.

    /// <summary>
    /// Markers used to indicate the state of the a cluster or sector on the disk
    /// In the case of a sector, this is just the first cluster on the disk which
    /// has the additional possible state of having the FormattedSector marker.
    /// </summary>
    public partial class TinyFileSystem
    {
        internal static class BlockMarkers
        {
            public const Byte ErasedSector = 0xff;
            public const Byte FormattedSector = 0x7f;
            public const Byte PendingCluster = 0x3f;
            public const Byte AllocatedCluster = 0x1f;
            public const Byte OrphanedCluster = 0x0f;

            public static Byte[] FormattedSectorBytes = {FormattedSector};
            public static Byte[] PendingClusterBytes = {PendingCluster};
            public static Byte[] AllocatedClusterBytes = {AllocatedCluster};
            public static Byte[] OrphanedClusterBytes = {OrphanedCluster};
        }

        /// <summary>
        /// In memory representation of a file on "disk".
        /// This structure tracks the files total size, the clusters 
        /// that make up the content of the file and the number of currently open
        /// Streams on the file.
        /// </summary>
        public class FileRef
        {
            /// <summary>
            /// Unique object id of the file.
            /// </summary>
            public UInt16 ObjId;

            /// <summary>
            /// Size of the file in bytes.
            /// </summary>
            public Int32 FileSize;

            /// <summary>
            /// Number of open streams on the file.
            /// </summary>
            public Byte OpenCount;

            /// <summary>
            /// The list of clusters that make up the file content.
            ///
            /// Each block is sequenced in the order that the data occurs in the file.
            /// For example, index 0's value is the  cluster id of the disk location that 
            /// storing the data for the first block of file data, index 1 points to the 
            /// cluster id of the second block of data etc.
            /// <remarks>
            /// Block 0 also contains the files meta data in the form of a FileClusterBlock,
            /// subsequent block will be in the form of DataClusterBlocks. As a space optimization
            /// the FileClusterBlock also contain the initial data in the file.
            /// </remarks>
            /// </summary>
            public UInt16Array Blocks = new UInt16Array();
        }

        /// <summary>
        /// Statistics for the device
        /// </summary>
        public struct DeviceStats
        {
            /// <summary>
            /// Free memory available for use.
            /// </summary>
            /// <remarks>
            /// The free memory is reported in multiples of free cluster sizes. The unused space
            /// on currently allocated clusters is not reported. This also excludes any potential 
            /// free space that is currently occupied by orphaned clusters.
            /// </remarks>
            public readonly Int32 BytesFree;

            /// <summary>
            /// Memory occupied by orphaned clusters.
            /// </summary>
            /// <remarks>
            /// This counter will report the amount of space currently allocated to orphaned clusters.
            /// Compacting the file system will return this memory to the free pool.
            /// </remarks>
            public readonly Int32 BytesOrphaned;

            /// <summary>
            /// Creates an instance of the DeviceStats structure.
            /// </summary>
            /// <param name="bytesFree">Bytes free in the file system.</param>
            /// <param name="bytesOrphaned">Bytes orphaned in the file system.</param>
            public DeviceStats(Int32 bytesFree, Int32 bytesOrphaned)
            {
                BytesFree = bytesFree;
                BytesOrphaned = bytesOrphaned;
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override String ToString() => "Bytes Free: " + BytesFree + "\r\n" +
                       "Bytes Orphaned: " + BytesOrphaned;
        }

        /// <summary>
        /// Utility class used to serialize/deserialize the data structures of the file system
        /// between the in memory representation and the on device representation.
        /// </summary>
        /// <remarks>
        /// If these structures are changed you need to be very careful about correctly updating
        /// the constants found in this structure.
        /// </remarks>
        private struct ClusterBuffer
        {
            public const Int32 MaxFileNameLength = 16;
            public const Int32 CommonHeaderSize = 1 + 2 + 2 + 2;
            public const Int32 FileClusterHeaderSize = CommonHeaderSize + 1 + MaxFileNameLength + 8;
            public const Int32 DataClusterHeaderSize = CommonHeaderSize;

            private const Int32 MarkerOffset = 0;
            private const Int32 ObjIdOffset = 1;
            private const Int32 BlockIdOffset = 3;
            private const Int32 DataLengthOffset = 5;
            public const Int32 FileNameLengthOffset = 7;
            private const Int32 FileNameOffset = 8;
            private const Int32 CreationTimeOffset = 24;

            private readonly Byte[] _buffer;
            private readonly Int32 _clusterSize;
            private Int32 _minWrite;

            /*
                        public int MinWrite
                        {
                            get { return _minWrite; }
                        }
            */

            public Int32 MaxWrite { get; set; }

            /*
                        public byte[] Buffer
                        {
                            get { return _buffer; }
                        }
            */

            public Int32 FileClusterMaxDataLength { get; }

            public Int32 DataClusterMaxDataLength { get; }

            public ClusterBuffer(Int32 clusterSize) :
                this()
            {
                _clusterSize = clusterSize;
                _buffer = new Byte[clusterSize];
                FileClusterMaxDataLength = clusterSize - FileClusterHeaderSize;
                DataClusterMaxDataLength = clusterSize - DataClusterHeaderSize;
            }

            public void Clear()
            {
                Array.Clear(_buffer, 0, _clusterSize);
                _minWrite = 0;
                MaxWrite = 0;
            }


            #region Get methods

            public Byte GetMarker() => Blitter.GetByte(_buffer, MarkerOffset);

            public UInt16 GetObjId() => Blitter.GetUInt16(_buffer, ObjIdOffset);

            public UInt16 GetBlockId() => Blitter.GetUInt16(_buffer, BlockIdOffset);

            public UInt16 GetDataLength() => Blitter.GetUInt16(_buffer, DataLengthOffset);

            private Byte GetFileNameLength() => Blitter.GetByte(_buffer, FileNameLengthOffset);

            public String GetFileName() => Blitter.GetString(_buffer, GetFileNameLength(), FileNameOffset);

            public DateTime GetCreationTime() => Blitter.GetDateTime(_buffer, CreationTimeOffset);

            public UInt16 GetDataStartOffset() => GetDataOffset(GetBlockId() == 0);

            private UInt16 GetDataOffset(Boolean isFileEntry) => (UInt16)(isFileEntry ? FileClusterHeaderSize : DataClusterHeaderSize);

            #endregion

            #region Set methods

            public void SetMarker(Byte value) => UpdateWriteRange(MarkerOffset, Blitter.ToBytes(_buffer, value, MarkerOffset));

            public void SetObjId(UInt16 value) => UpdateWriteRange(ObjIdOffset, Blitter.ToBytes(_buffer, value, ObjIdOffset));

            public void SetBlockId(UInt16 value) => UpdateWriteRange(BlockIdOffset, Blitter.ToBytes(_buffer, value, BlockIdOffset));

            public void SetDataLength(UInt16 value) => UpdateWriteRange(DataLengthOffset, Blitter.ToBytes(_buffer, value, DataLengthOffset));

            public void SetFileName(String value)
            {
                var byteLen = (Byte) Blitter.ToBytes(_buffer, value.ToUpper(), MaxFileNameLength, FileNameOffset);
                Blitter.ToBytes(_buffer, byteLen, FileNameLengthOffset);
                UpdateWriteRange(FileNameOffset, byteLen);
            }

            public void SetCreationTime(DateTime value) => UpdateWriteRange(CreationTimeOffset, Blitter.ToBytes(_buffer, value, CreationTimeOffset));

            public void SetData(Byte[] data, Int32 offset, Int32 destinationOffset, Int32 length)
            {
                var firstByteOffset = GetDataStartOffset() + destinationOffset;
                Array.Copy(data, offset, _buffer, firstByteOffset, length);
                UpdateWriteRange(firstByteOffset, length);
            }

            #endregion

            private void UpdateWriteRange(Int32 offset, Int32 length)
            {
                if (_minWrite > offset) _minWrite = offset;
                if (MaxWrite < offset + length) MaxWrite = offset + length;
            }

            public static implicit operator Byte[](ClusterBuffer o)
            {
                return o._buffer;
            }
        }

        /// <summary>
        /// A utility class to facilitate the serialization and deserialization
        /// of the data types used in the FileS System structures to and from memory.
        /// </summary>
        /// <remarks>
        /// This utility class is limited to the types required by the file system.
        /// GetXXX  - functions extract the data type XXX from the supplied buffer starting at the specified index
        /// ToBytes - function push the byte representation of the data type into the supplied byte buffer starting at the specified index.
        /// </remarks>
        private static class Blitter
        {
            #region GetXXX

            public static Byte GetByte(Byte[] buffer, Int32 index) => buffer[index];

            public static UInt16 GetUInt16(Byte[] buffer, Int32 index)
            {
                var b1 = buffer[index++];
                var b2 = buffer[index];
                return (UInt16) (b1 | (b2 << 8));
            }

/*
            public static int GetInt32(byte[] buffer, int index)
            {
                byte b1 = buffer[index++];
                byte b2 = buffer[index++];
                byte b3 = buffer[index++];
                byte b4 = buffer[index];
                return (ushort) (b1 | (b2 << 8) | (b3 << 16) | (b4 | 24));
            }
*/

            private static Int64 GetInt64(Byte[] buffer, Int32 index)
            {
                Int64 b1 = buffer[index++];
                Int64 b2 = buffer[index++];
                Int64 b3 = buffer[index++];
                Int64 b4 = buffer[index++];
                Int64 b5 = buffer[index++];
                Int64 b6 = buffer[index++];
                Int64 b7 = buffer[index++];
                Int64 b8 = buffer[index];

                return b1 | (b2 << 8) | (b3 << 16) | (b4 << 24) | (b5 << 32) | (b6 << 40) | (b7 << 48) | (b8 << 56);
            }

            public static DateTime GetDateTime(Byte[] buffer, Int32 index)
            {
                var ticks = GetInt64(buffer, index);
                return new DateTime(ticks);
            }

/*
            public static string GetString(byte[] buffer, int index)
            {
                ushort length = GetUInt16(buffer, index);
                return GetString(buffer, length, index);
            }
*/

            public static String GetString(Byte[] buffer, Int32 length, Int32 index)
            {
                var bytes = new Byte[length];
                Array.Copy(buffer, index, bytes, 0, length);
/*
                index += length;
*/
                return new String(Encoding.UTF8.GetChars(bytes));
            }

            #endregion

            #region ToBytes

            public static Int32 ToBytes(Byte[] buffer, Byte value, Int32 index)
            {
                buffer[index] = value;
                return 1;
            }

            public static Int32 ToBytes(Byte[] buffer, UInt16 value, Int32 index)
            {
                buffer[index++] = (Byte) (value & 0xff);
                buffer[index] = (Byte) ((value >> 8) & 0xff);
                return 2;
            }

/*
            public static int ToBytes(byte[] buffer, int value, int index)
            {
                buffer[index++] = (byte) (value & 0xff);
                buffer[index++] = (byte) ((value >> 8) & 0xff);
                buffer[index++] = (byte) ((value >> 16) & 0xff);
                buffer[index] = (byte) ((value >> 24) & 0xff);
                return 4;
            }
*/

            private static Int32 ToBytes(Byte[] buffer, Int64 value, Int32 index)
            {
                buffer[index++] = (Byte) (value & 0xff);
                buffer[index++] = (Byte) ((value >> 8) & 0xff);
                buffer[index++] = (Byte) ((value >> 16) & 0xff);
                buffer[index++] = (Byte) ((value >> 24) & 0xff);
                buffer[index++] = (Byte) ((value >> 32) & 0xff);
                buffer[index++] = (Byte) ((value >> 40) & 0xff);
                buffer[index++] = (Byte) ((value >> 48) & 0xff);
                buffer[index] = (Byte) ((value >> 56) & 0xff);
                return 8;
            }

            public static Int32 ToBytes(Byte[] buffer, DateTime value, Int32 index) => ToBytes(buffer, value.Ticks, index);

            /*
                        public static int ToBytes(byte[] buffer, string value, int index)
                        {
                            return ToBytes(buffer, value, ushort.MaxValue, index);
                        }
            */

            public static Int32 ToBytes(Byte[] buffer, String value, UInt16 maxLength, Int32 index)
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                var byteCount = bytes.Length;
                Array.Copy(bytes, 0, buffer, index, Math.Min(maxLength, byteCount));
/*
                index += byteCount;
*/
                return byteCount;
            }

            #endregion
        }
    }
}
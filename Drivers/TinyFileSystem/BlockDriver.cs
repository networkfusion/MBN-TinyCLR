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

namespace MBN.Modules
{
    public partial class TinyFileSystem
    {
// ReSharper disable once InconsistentNaming
        private class BlockDriver : IBlockDriver
        {
            private readonly Storage _storage;

            public BlockDriver(Storage storage, Int32 pagesPerCluster = 4)
            {
                ClusterSize = (UInt16)(pagesPerCluster * storage.PageSize);
                _storage = storage;
            }

            public void EraseChip() => _storage.EraseChip();

            public void EraseSector(Int32 sectorId) => _storage.EraseSector(sectorId, 1);

            public void Read(UInt16 clusterId, Int32 clusterOffset, Byte[] data, Int32 index, Int32 count)
            {
                var address = (clusterId*ClusterSize) + clusterOffset;
                _storage.ReadData(address, data, index, count);
            }

            public void Write(UInt16 clusterId, Int32 clusterOffset, Byte[] data, Int32 index, Int32 count)
            {
                var address = (clusterId*ClusterSize) + clusterOffset;
                _storage.WriteData(address, data, index, count);
            }

            public Int32 DeviceSize
            {
                get { return _storage.Capacity; }
            }

            public Int32 SectorSize
            {
                get { return _storage.SectorSize; }
            }

            public UInt16 ClusterSize { get; }
        }
    }
}

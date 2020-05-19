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

// System.IO.BufferedStream
// Updated to work with .NET Micro Framework
// Changes by:
//     Chris Taylor (taylorza) - 26/08/2012

// System.IO.BufferedStream
//
// Author:
//   Matt Kimball (matt@kimball.net)
//   Ville Palo <vi64pa@kolumbus.fi>
//
// Copyright (C) 2004 Novell (http://www.novell.com)
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;

namespace MBN.Modules
{
    public partial class TinyFileSystem
    {
        /// <summary>
        /// Main class for the BufferedStream object.
        /// </summary>
        internal sealed class BufferedStream : Stream
        {
            private readonly Stream _mStream;
            private Byte[] _mBuffer;
            private Int32 _mBufferPos;
            private Int32 _mBufferReadAhead;
            private Boolean _mBufferReading;
            private Boolean _disposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="BufferedStream"/> class with a default buffer size of 4096 bytes.
            /// </summary>
            /// <param name="stream">The stream.</param>
            public BufferedStream(Stream stream)
                : this(stream, 4096)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BufferedStream"/> class.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <param name="bufferSize">The buffer_size.</param>
            /// <exception cref="System.ArgumentNullException">stream</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">buffer_size &lt;= 0</exception>
            /// <exception cref="System.ObjectDisposedException">Cannot access a closed Stream.</exception>
            public BufferedStream(Stream stream, Int32 bufferSize)
            {
                if (stream == null)
                    throw new ArgumentNullException("stream");
                // LAMESPEC: documented as < 0
                if (bufferSize <= 0)
                    throw new ArgumentOutOfRangeException("bufferSize", "<= 0");
                if (!stream.CanRead && !stream.CanWrite)
                {
                    throw new ObjectDisposedException("Cannot access a closed Stream.");
                }

                _mStream = stream;
                _mBuffer = new Byte[bufferSize];
            }

            public override Boolean CanRead
            {
                get { return _mStream.CanRead; }
            }

            public override Boolean CanWrite
            {
                get { return _mStream.CanWrite; }
            }

            public override Boolean CanSeek
            {
                get { return _mStream.CanSeek; }
            }

            public override Int64 Length
            {
                get
                {
                    Flush();
                    return _mStream.Length;
                }
            }

            public override Int64 Position
            {
                get
                {
                    CheckObjectDisposedException();
                    return _mStream.Position - _mBufferReadAhead + _mBufferPos;
                }

                set
                {
                    if (value < Position && (Position - value <= _mBufferPos) && _mBufferReading)
                    {
                        _mBufferPos -= (Int32) (Position - value);
                    }
                    else if (value > Position && (value - Position < _mBufferReadAhead - _mBufferPos) &&
                             _mBufferReading)
                    {
                        _mBufferPos += (Int32) (value - Position);
                    }
                    else
                    {
                        Flush();
                        _mStream.Position = value;
                    }
                }
            }

            public override void Close()
            {
                try
                {
                    if (_mBuffer != null)
                        Flush();
                }
                finally
                {
                    _mStream.Close();
                    _mBuffer = null;
                    _disposed = true;
                }
            }

            public override void Flush()
            {
                CheckObjectDisposedException();

                if (_mBufferReading)
                {
                    if (CanSeek)
                        _mStream.Position = Position;
                }
                else if (_mBufferPos > 0)
                {
                    _mStream.Write(_mBuffer, 0, _mBufferPos);
                }

                _mBufferReadAhead = 0;
                _mBufferPos = 0;
            }

            public override Int64 Seek(Int64 offset, SeekOrigin origin)
            {
                CheckObjectDisposedException();
                if (!CanSeek)
                {
                    throw new NotSupportedException("Non seekable stream.");
                }
                Flush();
                return _mStream.Seek(offset, origin);
            }

            public override void SetLength(Int64 value)
            {
                CheckObjectDisposedException();

                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                if (!_mStream.CanWrite && !_mStream.CanSeek)
                    throw new NotSupportedException("the stream cannot seek nor write.");

                if ((_mStream == null) || (!_mStream.CanRead && !_mStream.CanWrite))
                    throw new IOException("the stream is not open");

                _mStream.SetLength(value);
                if (Position > value)
                    Position = value;
            }

            public override Int32 ReadByte()
            {
                CheckObjectDisposedException();

                var b = new Byte[1];

                return Read(b, 0, 1) == 1 ? b[0] : -1;
            }

            public override void WriteByte(Byte value)
            {
                CheckObjectDisposedException();
                var b = new Byte[1];

                b[0] = value;
                Write(b, 0, 1);
            }

            public override Int32 Read(Byte[] array, Int32 offset, Int32 count)
            {
                if (array == null)
                    throw new ArgumentNullException("array");
                CheckObjectDisposedException();
                if (!_mStream.CanRead)
                {
                    throw new NotSupportedException("Cannot read from stream");
                }
                if (offset < 0)
                    throw new ArgumentOutOfRangeException("offset", "< 0");
                if (count < 0)
                    throw new ArgumentOutOfRangeException("count", "< 0");
                // re-ordered to avoid possible integer overflow
                if (array.Length - offset < count)
                    throw new ArgumentException("array.Length - offset < count");

                if (!_mBufferReading)
                {
                    Flush();
                    _mBufferReading = true;
                }

                if (count <= _mBufferReadAhead - _mBufferPos)
                {
                    Array.Copy(_mBuffer, _mBufferPos, array, offset, count);

                    _mBufferPos += count;
                    if (_mBufferPos == _mBufferReadAhead)
                    {
                        _mBufferPos = 0;
                        _mBufferReadAhead = 0;
                    }

                    return count;
                }

                var ret = _mBufferReadAhead - _mBufferPos;
                Array.Copy(_mBuffer, _mBufferPos, array, offset, ret);
                _mBufferPos = 0;
                _mBufferReadAhead = 0;
                offset += ret;
                count -= ret;

                if (count >= _mBuffer.Length)
                {
                    ret += _mStream.Read(array, offset, count);
                }
                else
                {
                    _mBufferReadAhead = _mStream.Read(_mBuffer, 0, _mBuffer.Length);

                    if (count < _mBufferReadAhead)
                    {
                        Array.Copy(_mBuffer, 0, array, offset, count);
                        _mBufferPos = count;
                        ret += count;
                    }
                    else
                    {
                        Array.Copy(_mBuffer, 0, array, offset, _mBufferReadAhead);
                        ret += _mBufferReadAhead;
                        _mBufferReadAhead = 0;
                    }
                }

                return ret;
            }

            public override void Write(Byte[] array, Int32 offset, Int32 count)
            {
                if (array == null)
                    throw new ArgumentNullException("array");
                CheckObjectDisposedException();
                if (!_mStream.CanWrite)
                {
                    throw new NotSupportedException("Cannot write to stream");
                }
                if (offset < 0)
                    throw new ArgumentOutOfRangeException("offset", "< 0");
                if (count < 0)
                    throw new ArgumentOutOfRangeException("count", "< 0");
                // avoid possible integer overflow
                if (array.Length - offset < count)
                    throw new ArgumentException("array.Length - offset < count");

                if (_mBufferReading)
                {
                    Flush();
                    _mBufferReading = false;
                }

                // reordered to avoid possible integer overflow
                if (_mBufferPos >= _mBuffer.Length - count)
                {
                    Flush();
                    _mStream.Write(array, offset, count);
                }
                else
                {
                    Array.Copy(array, offset, _mBuffer, _mBufferPos, count);
                    _mBufferPos += count;
                }
            }

            private void CheckObjectDisposedException()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("Stream is closed");
                }
            }
        }
    }
}

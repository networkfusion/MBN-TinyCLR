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
        /// <summary>
        /// Dynamically growing array of UInt16 (ushort) elements.    
        /// </summary>
        public class UInt16Array
        {
            private const Int32 DefaultCapacity = 4;
            private Int32 _capacity = DefaultCapacity;
            private UInt16[] _array = new UInt16[DefaultCapacity];

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get or set.</param>
            /// <returns>The element a the specified index.</returns>
            public UInt16 this[Int32 index]
            {
                get
                {
                    if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");
                    return _array[index];
                }
                set { Set(index, value); }
            }

            /// <summary>
            /// Gets the number of elements contained in the array.
            /// </summary>
            public Int32 Count { get; private set; }

            /// <summary>
            /// Adds an element to the end of the collection.
            /// </summary>
            /// <param name="value">Value of the element to add.</param>
            /// <returns>The new count of items in the array.</returns>
            public Int32 Add(UInt16 value)
            {
                if (Count == _capacity)
                {
                    Grow(_capacity << 1);
                }
                _array[Count++] = value;
                return Count;
            }

            /// <summary>
            /// Adjusts the length of the array. 
            /// This can be used to trim the end of the array.
            /// </summary>
            /// <param name="length">New length of the array.</param>
            public void SetLength(Int32 length)
            {
                if (length < 0) throw new ArgumentOutOfRangeException("length");
                if (length > Count)
                {
                    if (length > _capacity)
                        Grow(DefaultCapacity +
                             (Int32) Math.Ceiling((Double) length/DefaultCapacity)*DefaultCapacity);
                }
                Count = length;
            }

            /// <summary>
            /// Sets the value of an element at the specified index.
            /// If the index is beyond the end of the array, the array will grow 
            /// to accomodate the new element.
            /// </summary>
            /// <param name="index">The zero-based index of the element to set.</param>
            /// <param name="value"></param>
            private void Set(Int32 index, UInt16 value)
            {
                if (index < 0) throw new ArgumentOutOfRangeException("index");
                if (index >= _capacity)
                    Grow(DefaultCapacity + (Int32) Math.Ceiling((Double) index/DefaultCapacity)*DefaultCapacity);
                _array[index] = value;
                if (index >= Count) Count = index + 1;
            }

            /// <summary>
            /// Grows the internal array to increase the capacity.
            /// </summary>
            /// <param name="newSize">New size of the array.</param>
            private void Grow(Int32 newSize)
            {
                var newArray = new UInt16[newSize];
                Array.Copy(_array, newArray, _capacity);
                _capacity = newArray.Length;
                _array = newArray;
            }
        }
    }
}

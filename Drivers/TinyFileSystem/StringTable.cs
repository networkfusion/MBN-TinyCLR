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
        /// Table of commonly used strings in the Tiny File System.
        /// </summary>
        internal static class StringTable
        {
/*
            public const string Error_ReadPastEnd = "Read past end";
*/
            public const String Error_WritePastEnd = "Write past end";
            public const String Error_OutOfBounds = "Out of bounds";
            public const String Error_FileNotFound = "File not found";
            public const String Error_FileAlreadyExists = "File already exists";
            public const String Error_FileIsInUse = "File is in use";
            public const String Error_FileClosed = "File closed";
            public const String Error_NotMounted = "File system has not been mounted.";
            public const String Error_NotFormatted = "Not formatted";
            public const String Error_DiskFull = "Disk full";
        }
    }
}

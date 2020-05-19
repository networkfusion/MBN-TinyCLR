/*
 * SpeakUpClick driver for TinyCLR 2.0
 * 
 * Initial revision
 *  
 * Copyright 2020 MikroBus.Net
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. See the License for the specific language governing permissions and limitations under the License.
 * 
 */
using System;

namespace MBN.Modules
{
    public sealed partial class SpeakUpClick
    {
        /// <summary>
        /// Delegate for the SpeakDetected event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeakUpEventArgs"/> instance containing the event data.</param>
        public delegate void SpeakUpEventHandler(Object sender, SpeakUpEventArgs e);

        /// <summary>
        /// Class holding arguments for the SpeakDetected event.
        /// </summary>
        public class SpeakUpEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpeakUpEventArgs"/> class.
            /// </summary>
            /// <param name="command">Index of the detected command</param>
            public SpeakUpEventArgs(Byte command)
            {
                Command = command;
            }

            /// <summary>
            /// Gets the index of the detected command.
            /// </summary>
            /// <value>
            /// Index of the command, as recorded in the SpeakUp board
            /// </value>
            public Byte Command { get; private set; }
        }
    }
}
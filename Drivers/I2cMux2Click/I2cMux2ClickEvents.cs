/*
 * I2cMux2Click board driver for TinyCLR 2.0
 * 
 * Version 1.0 :
 *  - Initial revision
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
    public sealed partial class I2cMux2Click
    {
        /// <summary>
        /// Event handler for the InterruptDetected event
        /// </summary>
        /// <param name="sender">The I2CMux Click.</param>
        /// <param name="e">The <see cref="InterruptEventArgs"/> instance containing the event data.</param>
        public delegate void InterruptEventHandler(Object sender, InterruptEventArgs e);

        /// <summary>
        /// Class holding arguments for the InterruptDetected event.
        /// </summary>
        public class InterruptEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InterruptEventArgs"/> class.
            /// </summary>
            /// <param name="intChannels">Channels that threw interrupt.</param>
            public InterruptEventArgs(Byte intChannels)
            {
                Channels = intChannels;
            }

            /// <summary>
            /// Gets the channels that threw interrupt. 
            /// </summary>
            /// <value>
            /// 4-bit value, each bit indicating the channel on which the interrupt occured
            /// </value>
            public Byte Channels { get; private set; }
        }
    }
}
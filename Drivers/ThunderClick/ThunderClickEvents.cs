/*
 * Thunder Click board driver for TinyCLR 2.0
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
    public sealed partial class ThunderClick
    {
        /// <summary>
        /// Delegate for the LightningDetected event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LightningEventArgs"/> instance containing the event data.</param>
        public delegate void LightningEventHandler(Object sender, LightningEventArgs e);

        /// <summary>
        /// Delegate for the DisturbanceDetected and NoiseDetected events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public delegate void EventHandler(Object sender, EventArgs e);

        /// <summary>
        /// Class holding arguments for the ThunderDetected event.
        /// </summary>
        public class LightningEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="LightningEventArgs"/> class.
            /// </summary>
            /// <param name="distance">Distance of the detected lightning.</param>
            /// <param name="energy">Energy of the detected lightning. This value is just a pure number and has no physical meaning.</param>
            public LightningEventArgs(Int32 distance, Int32 energy)
            {
                Distance = distance;
                Energy = energy;
            }

            /// <summary>
            /// Gets the distance of the detected lightning.
            /// </summary>
            /// <value>
            /// The distance in kilokmeters. Max distance is 40 km.
            /// </value>
            public Int32 Distance { get; private set; }

            /// <summary>
            /// Gets the energy of the detected lightning.
            /// </summary>
            /// <value>
            /// This value is just a pure number and has no physical meaning.
            /// </value>
            public Int32 Energy { get; private set; }
        }
    }
}
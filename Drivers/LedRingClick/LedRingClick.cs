/*
 * Led Ring Click board driver for TinyCLR 2.0
 * 
 * Version 1.0
 * - Initial revision
 *  
 * Copyright 2020 MikroBus.Net
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */

using GHIElectronics.TinyCLR.Devices.Spi;
using System;
using System.Threading;
using gpio = GHIElectronics.TinyCLR.Devices.Gpio;

namespace MBN.Modules
{
    public sealed class LedRingClick
	{
        private readonly SpiDevice _ledRing;
        private readonly Byte[] buffer = new Byte[4];

        public LedRingClick(Hardware.Socket socket)
        {
            gpio.GpioPin _rst = gpio.GpioController.GetDefault().OpenPin(socket.Rst);
            _rst.SetDriveMode(gpio.GpioPinDriveMode.Output);
            _rst.Write(gpio.GpioPinValue.High);

            _ledRing = SpiController.FromName(socket.SpiBus).GetDevice(new SpiConnectionSettings()
            {
                ChipSelectType = SpiChipSelectType.Gpio,
                ChipSelectLine = gpio.GpioController.GetDefault().OpenPin(socket.Cs),
                Mode = SpiMode.Mode3,
                ClockFrequency = 1000000
            });

            _rst.Write(gpio.GpioPinValue.Low);
            Thread.Sleep(1);
            _rst.Write(gpio.GpioPinValue.High);
        }

        public void Write(UInt32 data)
        {
            buffer[0] = (Byte)data;
            buffer[1] = (Byte)(data >> 8);
            buffer[2] = (Byte)(data >> 16);
            buffer[3] = (Byte)(data >> 24);
            lock (Hardware.LockSPI)
            {
                _ledRing.Write(buffer);
            }
        }

        public void Write (Double value)
        {
            if (value < 0.0 || value > 1.0) return;
            Write(value == 1.0 ? 0xFFFFFFFF : (UInt32)(1 << (Int32)(32*value)) - 1);
        }

        public void SetPosition(Byte pos, Boolean fill = false) => Write(pos == 32 && fill ? 0xFFFFFFFF : (UInt32)((1 << pos) - (fill ? 1 : 0)));
    }
}

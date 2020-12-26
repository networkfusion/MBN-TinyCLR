/* 
* MikroBus.Net main assembly for STM32F769I-Disco on nanoFramework
* 
* Version 0.1 : 
* - Initial revision
* 
* Copyright 2020 MikroBus.Net, NetworkFusion. 
* Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at 
* http://www.apache.org/licenses/LICENSE-2.0 
* Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
* either express or implied. See the License for the specific language governing permissions and limitations under the License. 
*/

using System.Device.Gpio;

using System;
using System.Collections;

namespace MBN
{

    #region Hardware Class
    /// <summary>
    /// Main Hardware class for MikroBus.Net board
    /// </summary>
    public static class Hardware
    {
        #region Socket Class
        /// <summary>
        /// MikroBus socket description
        /// </summary>
        public class Socket
        {
            /// <summary>
            /// Analog pin
            /// </summary>
            public Int32 AnPin;

            /// <summary>
            /// Analog Channel
            /// </summary>
            public Int32 AdcChannel;

            /// <summary>
            /// ADC Controller
            /// </summary>
            public String AdcController;

            /// <summary>
            /// UART COM port
            /// </summary>
            public String ComPort;

            /// <summary>
            /// Chip Select pin for SPI
            /// </summary>
            public Int32 Cs;

            /// <summary>
            /// Interrupt pin
            /// </summary>
            public Int32 Int;

            /// <summary>
            /// SPI Master IN Slave OUT pin
            /// </summary>
            public Int32 Miso;

            /// <summary>
            /// SPI Master OUT Slave IN pin
            /// </summary>
            public Int32 Mosi;

            /// <summary>
            /// Socket's name
            /// </summary>
            public String Name;

            /// <summary>
            /// PWM Controller
            /// </summary>
            public String PwmController;

            /// <summary>
            /// PWM channel
            /// </summary>
            public Int32 PwmChannel;

            /// <summary>
            /// PWM pin
            /// </summary>
            public Int32 PwmPin;

            /// <summary>
            /// Reset pin
            /// </summary>
            public Int32 Rst;

            /// <summary>
            /// UART Receive pin
            /// </summary>
            public Int32 Rx;

            /// <summary>
            /// SPI Clock line
            /// </summary>
            public Int32 Sck;

            /// <summary>
            /// I²C Serial Clock
            /// </summary>
            public Int32 Scl;

            /// <summary>
            /// I²C Serial Data
            /// </summary>
            public Int32 Sda;

            /// <summary>
            /// SPI module
            /// </summary>
            public String SpiBus;

            /// <summary>
            /// I2C Bus
            /// </summary>
            public String I2cBus;

            /// <summary>
            /// SPI bus lock object
            /// </summary>
            public Object LockSpi;

            /// <summary>
            /// I2C Bus lock object
            /// </summary>
            public Object LockI2c;

            /// <summary>
            /// UART Transmit pin
            /// </summary>
            public Int32 Tx;
        }
        #endregion

        #region Click sockets definitions

        #region STM32F769I board definitions
        public static class Stm32f769iDisco
        {
            /// <summary>ADC channel definitions.</summary>
            public static class AdcChannel
            {
                /// <summary>ADC controller.</summary>
                public static class Controller1
                {
                    public const String Id = STM32H7.AdcChannel.Adc1;

                    public const Int32 PA0C = STM32H7.AdcChannel.Channel0;
                    public const Int32 PA1C = STM32H7.AdcChannel.Channel1;
                    public const Int32 PA6 = STM32H7.AdcChannel.Channel3;
                    public const Int32 PB1 = STM32H7.AdcChannel.Channel5;
                    public const Int32 PA7 = STM32H7.AdcChannel.Channel7;
                    public const Int32 PB0 = STM32H7.AdcChannel.Channel9;
                    public const Int32 PC0 = STM32H7.AdcChannel.Channel10;
                    public const Int32 PC2 = STM32H7.AdcChannel.Channel12;
                    public const Int32 PC3 = STM32H7.AdcChannel.Channel13;
                    public const Int32 PA3 = STM32H7.AdcChannel.Channel15;
                    public const Int32 PA0 = STM32H7.AdcChannel.Channel16;
                    public const Int32 PA4 = STM32H7.AdcChannel.Channel18;
                    public const Int32 PA5 = STM32H7.AdcChannel.Channel19;
                }
                /// <summary>ADC controller.</summary>
                public static class Controller3
                {
                    public const String Id = STM32H7.AdcChannel.Adc3;

                    public const Int32 PC2C = STM32H7.AdcChannel.Channel0;
                    public const Int32 PC3C = STM32H7.AdcChannel.Channel1;
                    public const Int32 PF9 = STM32H7.AdcChannel.Channel2;
                    public const Int32 PF7 = STM32H7.AdcChannel.Channel3;
                    public const Int32 PF10 = STM32H7.AdcChannel.Channel6;
                    public const Int32 PF8 = STM32H7.AdcChannel.Channel7;
                    public const Int32 PF6 = STM32H7.AdcChannel.Channel8;
                    public const Int32 PH4 = STM32H7.AdcChannel.Channel15;
                }
            }

            /// <summary>DAC channel definitions.</summary>
            public static class DacChannel
            {
                public const String Id = STM32H7.DacChannel.Id;

                public const Int32 PA4 = STM32H7.DacChannel.Channel1;
                public const Int32 PA5 = STM32H7.DacChannel.Channel2;
            }

            // /// <summary>Network controller definitions.</summary>
            // public static class NetworkController
            // {
            //     public const String ATWinc15x0 = "GHIElectronics.TinyCLR.NativeApis.ATWINC15xx.NetworkController";
            //     public const String Enc28j60 = "GHIElectronics.TinyCLR.NativeApis.ENC28J60.NetworkController";
            //     public const String Ppp = "GHIElectronics.TinyCLR.NativeApis.Ppp.NetworkController";
            // }

            /// <summary>PWM pin definitions.</summary>
            public static class PwmChannel
            {
                /// <summary>PWM controller 1.</summary>
                public static class Controller1
                {
                    public const String Id = STM32H7.PwmChannel.Tim1;

                    public const Int32 PK1 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PJ11 = STM32H7.PwmChannel.Channel1;
                    public const Int32 PJ9 = STM32H7.PwmChannel.Channel2;

                }

                /// <summary>PWM controller 2.</summary>
                public static class Controller2
                {
                    public const String Id = STM32H7.PwmChannel.Tim2;

                    public const Int32 PA15 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PB3 = STM32H7.PwmChannel.Channel1;
                    public const Int32 PA3 = STM32H7.PwmChannel.Channel3;
                }

                /// <summary>PWM controller 3.</summary>
                public static class Controller3
                {
                    public const String Id = STM32H7.PwmChannel.Tim3;

                    public const Int32 PC6 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PC7 = STM32H7.PwmChannel.Channel1;
                    public const Int32 PB0 = STM32H7.PwmChannel.Channel2;
                    public const Int32 PB1 = STM32H7.PwmChannel.Channel3;
                }

                /// <summary>PWM controller 4.</summary>
                public static class Controller4
                {
                    public const String Id = STM32H7.PwmChannel.Tim4;

                    public const Int32 PB7 = STM32H7.PwmChannel.Channel1;

                }

                /// <summary>PWM controller 5.</summary>
                public static class Controller5
                {
                    public const String Id = STM32H7.PwmChannel.Tim5;

                    public const Int32 PA0 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PH11 = STM32H7.PwmChannel.Channel1;
                    public const Int32 PH12 = STM32H7.PwmChannel.Channel2;
                    public const Int32 PI0 = STM32H7.PwmChannel.Channel3;
                }

                /// <summary>PWM controller 8.</summary>
                public static class Controller8
                {
                    public const String Id = STM32H7.PwmChannel.Tim8;

                    public const Int32 PI5 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PI6 = STM32H7.PwmChannel.Channel1;
                    public const Int32 PI7 = STM32H7.PwmChannel.Channel2;
                    public const Int32 PI2 = STM32H7.PwmChannel.Channel3;
                }

                /// <summary>PWM controller 12.</summary>
                public static class Controller12
                {
                    public const String Id = STM32H7.PwmChannel.Tim12;

                    public const Int32 PH6 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PH9 = STM32H7.PwmChannel.Channel1;
                }

                /// <summary>PWM controller 13.</summary>
                public static class Controller13
                {
                    public const String Id = STM32H7.PwmChannel.Tim13;

                    public const Int32 PF8 = STM32H7.PwmChannel.Channel0;
                }

                /// <summary>PWM controller 14.</summary>
                public static class Controller14
                {
                    public const String Id = STM32H7.PwmChannel.Tim14;

                    public const Int32 PF9 = STM32H7.PwmChannel.Channel0;
                }

                /// <summary>PWM controller 15.</summary>
                public static class Controller15
                {
                    public const String Id = STM32H7.PwmChannel.Tim15;

                    public const Int32 PE5 = STM32H7.PwmChannel.Channel0;
                    public const Int32 PE6 = STM32H7.PwmChannel.Channel1;
                }

                /// <summary>PWM controller 16.</summary>
                public static class Controller16
                {
                    public const String Id = STM32H7.PwmChannel.Tim16;

                    public const Int32 PB8 = STM32H7.PwmChannel.Channel0;
                }

                /// <summary>PWM controller 17.</summary>
                public static class Controller17
                {
                    public const String Id = STM32H7.PwmChannel.Tim17;

                    public const Int32 PB9 = STM32H7.PwmChannel.Channel0;
                }
            }

            /// <summary>UART port definitions.</summary>
            public static class UartPort
            {
                /// <summary>UART port on PA9 (TX) and PA10 (RX).</summary>
                public const String Usart1 = STM32H7.UartPort.Usart1;

                /// <summary>UART port on PD5 (TX) and PD6 (RX), PD3 (CTS) and PD4 (RTS).</summary>
                public const String Usart2 = STM32H7.UartPort.Usart2;

                /// <summary>UART port on PC10 (TX) and PC11 (RX).</summary>
                public const String Usart3 = STM32H7.UartPort.Usart3;

                /// <summary>UART port on PH13 (TX) and PH14 (RX), PB0 (CTS) and PA15 (RTS).</summary>
                public const String Uart4 = STM32H7.UartPort.Uart4;

                /// <summary>UART port on PB13 (TX) and PB12 (RX), PC9 (CTS) and PC8 (RTS).</summary>
                public const String Uart5 = STM32H7.UartPort.Uart5;

                /// <summary>UART port on PC6 (TX) and PC7 (RX).</summary>
                public const String Usart6 = STM32H7.UartPort.Usart6;

                /// <summary>UART port on PF7 (TX) and PF6 (RX), PF9 (CTS) and PF8 (RTS).</summary>
                public const String Uart7 = STM32H7.UartPort.Uart7;

                /// <summary>UART port on PJ8 (TX) and PJ9 (RX).</summary>
                public const String Uart8 = STM32H7.UartPort.Uart8;
            }

            /// <summary>I2C bus definitions.</summary>
            public static class I2cBus
            {
                /// <summary>I2C bus on PB9 (SDA) and PB8 (SCL).</summary>
                public const String I2c1 = STM32H7.I2cBus.I2c1;
                /// <summary>I2C bus on PB11 (SDA) and PB10 (SCL).</summary>
                public const String I2c2 = STM32H7.I2cBus.I2c2;
                /// <summary>I2C bus on PH8 (SDA) and PH7 (SCL).</summary>
                public const String I2c3 = STM32H7.I2cBus.I2c3;
            }

            /// <summary>SPI bus definitions.</summary>
            public static class SpiBus
            {
                /// <summary>SPI bus on PI3 (MOSI), PI2 (MISO), and PI1 (SCK).</summary>
                public const String Spi2 = STM32H7.SpiBus.Spi2;
                /// <summary>SPI bus on PB5 (MOSI), PB4 (MISO), and PB3 (SCK).</summary>
                public const String Spi3 = STM32H7.SpiBus.Spi3;
                /// <summary>SPI bus on PJ10 (MOSI), PJ11 (MISO), and PK0 (SCK).</summary>
                public const String Spi5 = STM32H7.SpiBus.Spi5;
            }

            /// <summary>CAN bus definitions.</summary>
            public static class CanBus
            {
                /// <summary>CAN bus on PH13 (TX) and PH14 (RX).</summary>
                public const String Can1 = STM32H7.CanBus.Can1;
                /// <summary>CAN bus on PB13 (TX) and PB12 (RX).</summary>
                public const String Can2 = STM32H7.CanBus.Can2;
            }

            // /// <summary>Storage controller definitions.</summary>
            // public static class StorageController
            // {
            //     /// <summary>API id.</summary>
            //     public const String SdCard = STM32H7.StorageController.SdCard;
            //     public const String UsbHostMassStorage = STM32H7.StorageController.UsbHostMassStorage;
            //     //public const String QuadSpi = STM32H7.StorageController.QuadSpi;
            // }

            /// <summary>RTC controller definitions.</summary>
            public static class RtcController
            {
                /// <summary>API id.</summary>
                public const String Id = STM32H7.RtcController.Id;
            }
        }
        #endregion

        #region Arduino Uno Click Shield sockets
        /// <summary>
        /// Socket #1 on Arduino Uno Click Shield
        /// </summary>
        public static readonly Socket SocketOne = new Socket
        {
            AdcController = Stm32f769iDisco.AdcChannel.Controller1.Id,
            AdcChannel = Stm32f769iDisco.AdcChannel.Controller1.PC0,
            AnPin = STM32H7.GpioPin.PC0,
            Rst = STM32H7.GpioPin.PJ3,
            Cs = STM32H7.GpioPin.PJ4,
            SpiBus = Stm32f769iDisco.SpiBus.Spi2,
            Sck = STM32H7.GpioPin.PI1,
            Miso = STM32H7.GpioPin.PI2,
            Mosi = STM32H7.GpioPin.PI3,
            PwmController = Stm32f769iDisco.PwmChannel.Controller2.Id,
            PwmChannel = Stm32f769iDisco.PwmChannel.Controller2.PA15,
            PwmPin = STM32H7.GpioPin.PA15,
            Int = STM32H7.GpioPin.PJ5,
            ComPort = Stm32f769iDisco.UartPort.Usart6,
            Rx = STM32H7.GpioPin.PC7,
            Tx = STM32H7.GpioPin.PC6,
            I2cBus = Stm32f769iDisco.I2cBus.I2c2,
            Scl = STM32H7.GpioPin.PB10,
            Sda = STM32H7.GpioPin.PB11,
            Name = "SocketOne"
        };

        /// <summary>
        /// Socket #2 on Arduino Uno Adapter Click Shield
        /// </summary>
        public static readonly Socket SocketTwo = new Socket
        {
            AdcController = Stm32f769iDisco.AdcChannel.Controller1.Id,
            AdcChannel = Stm32f769iDisco.AdcChannel.Controller1.PC3,
            AnPin = STM32H7.GpioPin.PC3,
            Rst = STM32H7.GpioPin.PJ6,
            Cs = STM32H7.GpioPin.PH15,
            SpiBus = Stm32f769iDisco.SpiBus.Spi2,
            Sck = STM32H7.GpioPin.PI1,
            Miso = STM32H7.GpioPin.PI2,
            Mosi = STM32H7.GpioPin.PI3,
            PwmController = Stm32f769iDisco.PwmChannel.Controller1.Id,
            PwmChannel = Stm32f769iDisco.PwmChannel.Controller1.PK1,
            PwmPin = STM32H7.GpioPin.PK1,
            Int = STM32H7.GpioPin.PI15,
            ComPort = Stm32f769iDisco.UartPort.Usart2,
            Rx = STM32H7.GpioPin.PD6,
            Tx = STM32H7.GpioPin.PD5,
            I2cBus = Stm32f769iDisco.I2cBus.I2c2,
            Scl = STM32H7.GpioPin.PB10,
            Sda = STM32H7.GpioPin.PB11,
            Name = "SocketTwo"
        };

        #endregion


        #region Public objects
        /// <summary>
        /// RAM onboard Led 1 (Green)
        /// </summary>
        public static readonly GpioPin Led1;

        /// <summary>
        /// RAM onboard Led 2 (Orange)
        /// </summary>
        public static readonly GpioPin Led2;

        /// <summary>
        /// RAM onboard Led 3 (Red)
        /// </summary>
        public static readonly GpioPin Led3;

        /// <summary>
        /// SC20260D onboard Led 1 (Green)
        /// </summary>
        public static readonly GpioPin SC20260D_Led1;

        /// <summary>
        /// SC20260D onboard Led 2 (Red)
        /// </summary>
        public static readonly GpioPin SC20260D_Led2;

        /// <summary>
        /// Lock object used for I2C
        /// </summary>
        public static readonly Object LockI2C;

        /// <summary>
        /// Lock object used for SPI
        /// </summary>
        public static readonly Object LockSPI;
        #endregion

        #region .ctor
        // Constructor
        static Hardware()
        {
            SocketOne.LockI2c = new Object();
            SocketTwo.LockI2c = SocketOne.LockI2c;

            SocketOne.LockSpi = new Object();
            SocketTwo.LockSpi = SocketOne.LockSpi;


            LockI2C = new Object();
            LockSPI = new Object();
            Led1 = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PK4);
            Led1.SetDriveMode(GpioPinDriveMode.Output);
            Led1.Write(GpioPinValue.Low);
            Led2 = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PK5);
            Led2.SetDriveMode(GpioPinDriveMode.Output);
            Led2.Write(GpioPinValue.Low);
            Led3 = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PK6);
            Led3.SetDriveMode(GpioPinDriveMode.Output);
            Led3.Write(GpioPinValue.Low);
            
        }
        #endregion
    }
    #endregion

}

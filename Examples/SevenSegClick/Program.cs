using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;
using static System.Diagnostics.Debug;
using gpio = GHIElectronics.TinyCLR.Devices.Gpio;

namespace Examples
{
    class Program
    {
        private static SevenSegClick _seven;
        private static readonly Byte[] Spin1 =
        {
            0x00, 0x04,
            0x00, 0x40,
            0x00, 0x20,
            0x00, 0x10,
            0x10, 0x00,
            0x08, 0x00,
            0x02, 0x00,
            0x04, 0x00
        };

        private static readonly Byte[] Spin2 =
        {
            0x04, 0x04,
            0x80, 0x80,
            0x10, 0x10,
            0x80, 0x80
        };

        private static readonly Byte[] Spin3 =
        {
            0x00, 0x60,
            0x00, 0x0A,
            0x60, 0x00,
            0x0A, 0x00,
            0x60, 0x00,
            0x00, 0x0A
        };

        static void Main()
        {
            try
            {
                WriteLine("Program started");

                _seven = new SevenSegClick(Hardware.SocketFour, 0.05);

                // Displays from 0 to 9.9
                // Trick : no float here, only bytes, the dot is added as soon as i > 9
                for (Byte i = 0; i < 100; i++)
                {
                    _seven.SendBytes(i < 10
                        ? new Byte[] { _seven.GetDigit(i), 0x00 }
                        : new[] { _seven.GetDigit((Byte)(i % 10)), (Byte)(_seven.GetDigit((Byte)(i / 10)) + 1) });
                    Thread.Sleep(50);
                }
                Thread.Sleep(500);

                // Some fun now !
                for (var j = 0; j < 10; j++)
                {
                    for (Byte i = 0; i < 16; i += 2)
                    {
                        _seven.SendBytes(new[] { Spin1[i], Spin1[i + 1] });
                        Thread.Sleep(50);
                    }
                }

                for (var j = 0; j < 6; j++)
                {
                    for (Byte i = 0; i < 8; i += 2)
                    {
                        _seven.SendBytes(new[] { Spin2[i], Spin2[i + 1] });
                        Thread.Sleep(200);
                    }
                }

                for (var j = 0; j < 6; j++)
                {
                    for (Byte i = 0; i < 12; i += 2)
                    {
                        _seven.SendBytes(new[] { Spin3[i], Spin3[i + 1] });
                        Thread.Sleep(200);
                    }
                }
                _seven.Clear();

            }
            catch (Exception ex) when (Debugger.IsAttached)
            {
                WriteLine("Exception caught : " + ex.Message);
            }
            catch
            {
                while (true)
                {
                    Hardware.Led3.Write(Hardware.Led3.Read() ^ gpio.GpioPinValue.High);
                    Thread.Sleep(100);
                }
            }
            finally
            {
                WriteLine("Entering infinite loop...");
                while (true)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}

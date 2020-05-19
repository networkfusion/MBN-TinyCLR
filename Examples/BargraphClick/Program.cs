using static System.Diagnostics.Debug;
using gpio = GHIElectronics.TinyCLR.Devices.Gpio;
using System;
using System.Threading;
using MBN;
using System.Diagnostics;
using MBN.Modules;

namespace Examples
{
    class Program
    {
        private static BarGraphClick Leds;

        static void Main()
        {
            try
            {
                Leds = new BarGraphClick(Hardware.SocketOne);

                Demo(true);
                Demo(false);

                Leds.WriteMask(0b01_01010101);
                Thread.Sleep(2000);
                Leds.WriteMask(0b10_10101010);
                Thread.Sleep(2000);

                Leds.Brightness = 1.0;
                Leds.Bars(5);
                DemoBrightness();
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
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static void DemoBrightness()
        {
            for (var j = 0; j < 3; j++)
            {
                for (Double i = 0; i < 0.5; i += 0.01)
                {
                    Leds.Brightness = i;
                    Thread.Sleep(25);
                }
                for (var i = 0.5; i > 0; i -= 0.01)
                {
                    Leds.Brightness = i;
                    Thread.Sleep(25);
                }
            }
        }

        private static void Demo(Boolean Fill)
        {
            for (var j = 0; j < 3; j++)
            {
                for (UInt16 i = 0; i <= 10; i++)
                {
                    Leds.Bars(i, Fill);
                    Thread.Sleep(50);
                }
                for (UInt16 i = 0; i <= 10; i++)
                {
                    Leds.Bars((UInt16)(10 - i), Fill);
                    Thread.Sleep(50);
                }
            }
        }
    }
}

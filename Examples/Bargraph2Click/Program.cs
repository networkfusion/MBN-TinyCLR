using static System.Diagnostics.Debug;
using gpio = GHIElectronics.TinyCLR.Devices.Gpio;
using System;
using System.Threading;
using MBN;
using MBN.Modules;
using System.Diagnostics;

namespace Examples
{
    class Program
    {
        private static Bargraph2Click _bargraph2;

        private static void Main()
        {
            try
            {
                _bargraph2 = new Bargraph2Click(Hardware.SocketOne);

                #region SetMask demo
                _bargraph2.SetMask("GGGGGRRRRR");
                _bargraph2.Bars(10, true);
                Thread.Sleep(2000);
                _bargraph2.SetMask("RRRRROOOOO");
                _bargraph2.Bars(10, true);
                Thread.Sleep(2000);
                _bargraph2.SetMask("GGGGOOOORR");
                _bargraph2.Bars(10, true);
                Thread.Sleep(2000);
                _bargraph2.SetMask("RROOOGGGGG");
                _bargraph2.Bars(10, true);
                Thread.Sleep(2000);
                #endregion

                #region Leds() demo
                // One led at a time
                DemoBars(false);
                // Filled bars
                DemoBars(true);
                #endregion

                #region Write() demo
                // All leds green
                _bargraph2.Write(0b0000_0000000000_1111111111);
                Thread.Sleep(2000);
                // All leds orange
                _bargraph2.Write(0b0000_1111111111_1111111111);
                Thread.Sleep(2000);
                // All leds red
                _bargraph2.Write(0b0000_1111111111_0000000000);
                Thread.Sleep(2000);
                #endregion

                #region Brightness() demo
                DemoBrightness();
                #endregion
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

        private static void DemoBars(Boolean Fill)
        {
            for (var j = 0; j < 3; j++)
            {
                for (Byte i = 0; i <= 10; i++)
                {
                    _bargraph2.Bars(i, Fill);
                    Thread.Sleep(50);
                }
                for (Byte i = 0; i <= 10; i++)
                {
                    _bargraph2.Bars((Byte)(10 - i), Fill);
                    Thread.Sleep(50);
                }
            }
        }

        private static void DemoBrightness()
        {
            _bargraph2.SetMask("ROGROGROGR");
            _bargraph2.Bars(10, true);
            for (var j = 0; j < 3; j++)
            {
                for (Double i = 0; i < 0.5; i += 0.01)
                {
                    _bargraph2.Brightness = i;
                    Thread.Sleep(25);
                }
                for (var i = 0.5; i > 0; i -= 0.01)
                {
                    _bargraph2.Brightness = i;
                    Thread.Sleep(25);
                }
            }
            _bargraph2.Brightness = 0.0;
        }
    }
}

using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;
using static System.Diagnostics.Debug;

namespace Examples
{
    class Program
    {
        private static DevantechLcd03 _lcd;

        static void Main()
        {
            try
            {
                // I²C Mode
                //_lcd = new DevantechLcd03(Hardware.SocketOne, 0xC8 >> 1)

                // Serial mode
                _lcd = new DevantechLcd03(Hardware.SocketOne)
                {
                    BackLight = true,
                    Cursor = DevantechLcd03.Cursors.Hide
                };
                _lcd.ClearScreen();
                _lcd.Write(1, 1, "Hello world !");
                _lcd.Write(1, 2, "UART mode");
                _lcd.Write(1, 4, "Using TinyCLR 2.0");
            }

            catch (Exception ex) when (Debugger.IsAttached)
            {
                WriteLine("Exception caught : " + ex.Message);
            }
            catch
            {
            }
            finally
            {
                WriteLine("Entering infinite loop...");
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}

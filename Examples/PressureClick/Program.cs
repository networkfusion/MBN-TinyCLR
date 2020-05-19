using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        static PressureClick _pres;
        static Boolean _deviceOk;

        public static void Main()
        {
            _deviceOk = false;
            while (!_deviceOk)
            {
                try
                {
                    _pres = new PressureClick(Hardware.SocketOne, 0xBA >> 1);
                    _deviceOk = true;
                }
                catch (DeviceInitialisationException)
                {
                    Debug.WriteLine("Init failed, retrying...");
                }
            }

            while (true)
            {
                Debug.WriteLine("Pression = " + _pres.ReadPressure() + " hPa");
                Debug.WriteLine("Temperature = " + _pres.ReadTemperature().ToString("F2") + "°");
                Thread.Sleep(1000);
            }
        }
    }
}

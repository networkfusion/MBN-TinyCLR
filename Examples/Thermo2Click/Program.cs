using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public class Program
    {
        // Here i am using two (2) Thermo2 clicks in a stacked arrangement.
        // Code will work as is for a single device.
        private static Thermo2Click _thermo2;

        public static void Main()
        {
            // We only need to instantiate one (1) as both Thremo2 clicks are sharing the same One-Wire Bus.
            // The code will work with only one (1) Thermo2 Click as well.
            _thermo2 = new Thermo2Click(Hardware.SC20100_2, Thermo2Click.GpioSelect.GP0)
            {
                TemperatureUnit = TemperatureUnits.Celsius
            };

            _thermo2.SetResolutionForAllDevices(Thermo2Click.Resolution.Resolution12Bit, false);

            foreach (Byte[] device in _thermo2.DeviceList)
            {
                Debug.WriteLine($"Device ID - {GetDeviceId(device)}, Is Parasitic? - {_thermo2.IsParasitic(device)}");
            }

            new Thread(TempThread).Start();

            Thread.Sleep(-1);

        }

        private static void TempThread()
        {
            while (true)
            {
                foreach (Byte[] id in _thermo2.DeviceList)
                {
                    Debug.WriteLine($"Device Address - {GetDeviceId(id)}, S/N - {_thermo2.GetSerialNumber(id)}, Temperature - {_thermo2.ReadTemperatureByAddress(id)}");
                }

                /* For a single device, use the following code */
                //Debug.Print("Temperature - " + _thermo2.ReadTemperature());

                Thread.Sleep(1000);
            }
        }

        private static String GetDeviceId(Byte[] id)
        {
            return $"[0x{id[0]:x2}, 0x{id[1]:x2}, 0x{id[2]:x2}, 0x{id[3]:x2}, 0x{id[4]:x2}, 0x{id[5]:x2}, 0x{id[6]:x2}, 0x{id[7]:x2}]";
        }
    }
}

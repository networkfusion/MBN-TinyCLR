using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public class Program
    {
        private static Thermostat2Click _thermostat;

        public static void Main()
        {
            _thermostat = new Thermostat2Click(Hardware.SC20100_1) { TemperatureUnit = TemperatureUnits.Celsius };

            DisplayDeviceIds();

            _thermostat.SetResolutionForAllDevices(Thermostat2Click.Resolution.Resolution12Bit, false);

            _thermostat.SetAlarmsForAlllDevices(25, 29, false);

            Debug.WriteLine($"Number of devices on the 1-Wire bus is {_thermostat.NumberOfDevices}");

            while (true)
            {
                foreach (Byte[] sensor in _thermostat.DeviceList)
                {
                    Single result = _thermostat.ReadTemperatureByAddress(sensor);

                    if (_thermostat.HasAlarm(sensor))
                    {
                        TurnOnRelay();
                    }
                    else
                    {
                        TurnOffRelay();
                    }

                    Debug.WriteLine($"Device with ID of {GetDeviceId(sensor)} has a temperature of {result:f2} °C");

                    Thread.Sleep(1000);
                }
            }
        }

        private static void TurnOnRelay()
        {
            _thermostat.RelayState = true;
        }

        private static void TurnOffRelay()
        {
            _thermostat.RelayState = false;
        }

        private static void DisplayDeviceIds()
        {
            foreach (Byte[] device in _thermostat.DeviceList)
            {
                Debug.WriteLine(GetDeviceId(device));
            }
        }

        private static String GetDeviceId(Byte[] id)
        {
            return id[0] + ":" + id[1] + ":" + id[2] + ":" + id[3] + ":" + id[4] + ":" + id[5] + ":" + id[6] + ":" + id[7];
        }
    }
}

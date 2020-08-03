using System;
using MBN;
using MBN.Modules;

using System.Diagnostics;
using System.Threading;

namespace Examples
{
    internal class Program
    {
        private static TempHum11Click _sensor;

        private static void Main()
        {
            _sensor = new TempHum11Click(Hardware.SC20100_1) {TemperatureUnit = TemperatureUnits.Celsius};

            _sensor.Configure(TempHum11Click.AcquisitionMode.Sequential, TempHum11Click.TemperatureResolution.FourteenBit, TempHum11Click.HumidityResolution.FourteenBit, TempHum11Click.HeaterMode.Disabled);

            Debug.WriteLine($"Manufacturer ID 0x{_sensor.GetManufacturerId():x}");
            Debug.WriteLine($"Device ID - 0x{_sensor.GetDeviceId():X}");
            Debug.WriteLine($"SN - {_sensor.GetSerialNumber()}");

            while (true)
            {
                /* For Sequential Acquisition Mode use the following code to obtain temperature and humidity data
                   You will need to change the Acquisition Mode to Sequential.*/
                _sensor.ReadSensor(out Single temperature, out Single humidity);
                Debug.WriteLine($"Temperature : {temperature:F2} °C");
                Debug.WriteLine($"   Humidity : {humidity:F2} %RH\n");

                /* For independent measurement mode use the following code to obtain temperature and humidity data
                   You will need to change the Acquisition Mode to Independent.*/
                //Debug.WriteLine($"Temperature : {_sensor.ReadTemperature():F2} *C");
                //Debug.WriteLine($"   Humidity : {_sensor.ReadHumidity():F2} %RH\n");

                Thread.Sleep(2000);
            }
        }
    }
}

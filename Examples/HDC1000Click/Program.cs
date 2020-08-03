using MBN;
using MBN.Modules;

using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public static class Program
    {
        private static Hdc1000Click _sensor;

        public static void Main()
        {
            _sensor = new Hdc1000Click(Hardware.SC20100_2, Hdc1000Click.I2CAddress.I2CAddressTwo)
            {
                TemperatureUnit = TemperatureUnits.Fahrenheit
            };

            /* For Sequential Acquisition Mode use the following code to obtain temperature and humidity data */
            //_sensor.Configure(
            //    Hdc1000Click.AcquisitionMode.Sequential,
            //    Hdc1000Click.TemperatureResolution.FourteenBit,
            //    Hdc1000Click.HumidityResolution.FourteenBit,
            //    Hdc1000Click.HeaterMode.Disabled
            //    );

            /* For independent measurement mode use the following code to obtain temperature and humidity data */
            _sensor.Configure(
                Hdc1000Click.AcquisitionMode.Independant,
                Hdc1000Click.TemperatureResolution.FourteenBit,
                Hdc1000Click.HumidityResolution.FourteenBit,
                Hdc1000Click.HeaterMode.Disabled
            );

            Debug.WriteLine($"Manufacturer ID - 0x{_sensor.GetManufacturerId():X}");
            Debug.WriteLine($"Device ID - 0x{_sensor.GetDeviceId():X}");
            Debug.WriteLine($"SN - {_sensor.GetSerialNumber()}");

            while (true)
            {
                /* For Sequential Acquisition Mode use the following code to obtain temperature and humidity data */
                //_sensor.ReadSensor(out Single temperature, out Single humidity);
                //Debug.WriteLine($"Temperature - {temperature:F2} °F");
                //Debug.WriteLine($"   Humidity - {humidity:F2} %RH");

                /* For independent measurement mode use the following code to obtain temperature and humidity data */
                Debug.WriteLine($"Temperature - {_sensor.ReadTemperature():F2} °F");
                Debug.WriteLine($"   Humidity - {_sensor.ReadHumidity():F2} %RH");

                Thread.Sleep(2000);
            }
        }
    }
}

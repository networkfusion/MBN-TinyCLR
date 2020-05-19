using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    internal class Program
    {
        private static BME280 _sensor;

        private static void Main()
        {
            _sensor = new BME280(Hardware.SocketOne, BME280.I2CAddresses.Address0)
            {
                TemperatureUnit = TemperatureUnits.Fahrenheit,
                PressureCompensation = PressureCompensationModes.SeaLevelCompensated
            };

            _sensor.SetRecommendedMode(BME280.RecommendedModes.WeatherMonitoring);

            while (true)
            {
                Debug.WriteLine("------Reading individual values-------");

                Debug.WriteLine($"Pressure.......: {_sensor.ReadPressure():F1} hPa");
                Debug.WriteLine($"Temperature....: {_sensor.ReadTemperature():F2} °F");
                Debug.WriteLine($"Humidity.......: {_sensor.ReadHumidity():F2} %RH");
                Debug.WriteLine($"Altitude.......: {_sensor.ReadAltitude():F0} meters\n");

                _sensor.ReadSensor(out Single pressure, out Single temperature, out Single humidity, out Single altitude);

                Debug.WriteLine("------Using the ReadSensor Method-------");
                Debug.WriteLine($"Pressure.......: {pressure:F1} hPa");
                Debug.WriteLine($"Temperature....: {temperature:F2} °F");
                Debug.WriteLine($"Humidity.......: {humidity:F2} %RH");
                Debug.WriteLine($"Altitude.......: {altitude:F0} meters\n");

                Thread.Sleep(5000);
            }
        }
    }
}

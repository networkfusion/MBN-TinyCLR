using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    internal class Program
    {
        private static WeatherClick _sensor;

        private static void Main()
        {
            _sensor = new WeatherClick(Hardware.SC20100_1, WeatherClick.I2CAddresses.Address0)
            {
                TemperatureUnit = TemperatureUnits.Fahrenheit,
                PressureCompensation = PressureCompensationModes.SeaLevelCompensated
            };

            _sensor.SetRecommendedMode(WeatherClick.RecommendedModes.WeatherMonitoring);

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

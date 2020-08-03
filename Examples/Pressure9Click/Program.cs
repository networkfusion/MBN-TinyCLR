using System;
using System.Diagnostics;
using System.Threading;
using MBN;
using MBN.Modules;

namespace Examples
{
    internal class Program
    {
        private static Pressure9Click _sensor;

        private static void Main()
        {
            _sensor = new Pressure9Click(Hardware.SC20100_1, Pressure9Click.I2cAddresses.Address0)
            {
                PressureCompensation = PressureCompensationModes.SeaLevelCompensated,
                TemperatureUnit = TemperatureUnits.Fahrenheit
            };

            Pressure9Click.SensorConfiguration configuration = new Pressure9Click.SensorConfiguration
            {
                TemperatureMeasurementRate = Pressure9Click.TemperatureMeasurementRates.Hertz_8,
                TemperatureSamplingRate = Pressure9Click.OverSamplingRates.OSR_8X,
                PressureMeasurementRate = Pressure9Click.PressureMeasurementRates.Hertz_8,
                PressureSamplingRate = Pressure9Click.OverSamplingRates.OSR_8X,
                SensorMeasurementControl = Pressure9Click.MeasurementControl.ContinuousPressureandTemperatureMeasurement,
                FIFOEnable = true,
                FIFOBehavior = Pressure9Click.FIFOBehaviors.Streaming
            };

            _sensor.SetConfiguration(configuration);

            Debug.WriteLine($"Product ID is 0x{_sensor.ProductID:x2}");
            Debug.WriteLine($"Silicon Revision 0x{_sensor.SiliconRevision:x2}");

            while (true)
            {
                while (_sensor.ReadIFOFillLevel() < 32)
                {
                    Thread.Sleep(400); // Don't poll any faster than 375 ms per data-sheet.
                }

                Single[] data = _sensor.ReadFIFO();

                Debug.WriteLine("----------Pressure 9 Click----------");
                Debug.WriteLine($"Temperature.......: {data[0]:F2} *F");
                Debug.WriteLine($"Pressure..........: {data[1]:F2} Pa");
                Debug.WriteLine($"Altitude..........: {_sensor.Altitude:F1} meters\n");

                Thread.Sleep(1000);
            }
        }
    }
}

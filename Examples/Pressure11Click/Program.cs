using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public static class Program
    {
        private static Pressure11Click _sensor;
        private static Pressure11Click.SensorConfiguration configuration;

        public static void Main()
        {
            _sensor = new Pressure11Click(Hardware.SC20100_1, Pressure11Click.I2CAddress.AddressOne)
            {
                TemperatureUnit = TemperatureUnits.Fahrenheit,
                PressureCompensation = PressureCompensationModes.Uncompensated
            };

            Pressure11Click.SensorConfiguration config = new Pressure11Click.SensorConfiguration
            {
                OutputDataRate = Pressure11Click.ODR.OneHertz,
                FiFoEnabled = true,
                StopOnFiFoWatermark = true,
                FiFoWatermarkLevel = 16
            };

            _sensor.ConfigureSensor(config);

            /* Choose one of the following demos to run. */
            //FiFoDemo();
            //FiFoAverageDemo();
            OneShotDemo();
            //ContinuousMeasurementNoFiFoDemo();
        }

        private static void FiFoAverageDemo()
        {
            configuration = new Pressure11Click.SensorConfiguration
            {
                OutputDataRate = Pressure11Click.ODR.TenHertz,
                LowPassFilterConfiguration = Pressure11Click.LowPassFillter.Disabled,
                BlockDataUpdate = Pressure11Click.BDU.Continuous,

                FiFoEnabled = true,
                StopOnFiFoWatermark = false,
                AutoIncrementRegisterAddress = false,

                FiFoWatermarkLevel = 32,

                PowerModeConfiguration = Pressure11Click.PowerModes.LowNoise
            };

            _sensor.ConfigureSensor(configuration);

            while (true)
            {
                while (_sensor.GetFiFoFillLevel() < 32)
                {
                    Thread.Sleep(5);
                }

                Single[] fifoBuffer = _sensor.ReadFiFoAverage();
                Debug.WriteLine("---Pressure 9 Click FiFo Averaging Demo---");
                Debug.WriteLine("Pressure.......................: {fifoBuffer[0]:F2} mBar");
                Debug.WriteLine("Temperature....................: {fifoBuffer[1]:F2} °F");
                Debug.WriteLine("Altitude is....................: {_sensor.Altitude:F1} meters");
                Debug.WriteLine("Reference Pressure is..........: {_sensor.ReferencePressure:F2}");

                _sensor.ClearFiFoAndRestart();
            }
        }

        private static void FiFoDemo()
        {
            configuration = new Pressure11Click.SensorConfiguration
            {
                OutputDataRate = Pressure11Click.ODR.TenHertz,
                LowPassFilterConfiguration = Pressure11Click.LowPassFillter.High,
                BlockDataUpdate = Pressure11Click.BDU.Continuous,

                FiFoEnabled = true,
                StopOnFiFoWatermark = true,
                AutoIncrementRegisterAddress = true,

                FiFoWatermarkLevel = 31,

                PowerModeConfiguration = Pressure11Click.PowerModes.LowNoise
            };

            _sensor.ConfigureSensor(configuration);

            while (true)
            {
                while (_sensor.GetFiFoFillLevel() <= configuration.FiFoWatermarkLevel) Thread.Sleep(5);
                {
                    Single[][] fifoBuffer = _sensor.ReadFiFo();

                    for (Int32 x = 0; x < fifoBuffer.Length; x++)
                    {
                        Debug.WriteLine("-----Pressure 9 Click FiFo Demo-----");
                        Debug.WriteLine("Pressure.......................: {fifoBuffer[0]:F2} mBar");
                        Debug.WriteLine("Temperature....................: {fifoBuffer[1]:F2} °F");
                        Debug.WriteLine("Altitude is....................: {_sensor.Altitude:F1} meters");
                        Debug.WriteLine("Reference Pressure is..........: {_sensor.ReferencePressure:F2}");
                    }

                    _sensor.ClearFiFoAndRestart();
                }
            }
        }

        private static void OneShotDemo()
        {
            configuration = new Pressure11Click.SensorConfiguration
            {
                OutputDataRate = Pressure11Click.ODR.Powerdown,
                LowPassFilterConfiguration = Pressure11Click.LowPassFillter.Disabled,
                BlockDataUpdate = Pressure11Click.BDU.Quiescent,

                FiFoEnabled = false,
                StopOnFiFoWatermark = false,
                FiFoWatermarkLevel = 0,
                AutoIncrementRegisterAddress = true,

                PowerModeConfiguration = Pressure11Click.PowerModes.LowNoise
            };

            _sensor.ConfigureSensor(configuration);

            while (true)
            {
                Debug.WriteLine("---Pressure 9 Click One-shot Demo---");
                Debug.WriteLine("Pressure..............: {_sensor.ReadPressureOneshot():F2} mBar");
                Debug.WriteLine("Temperature...........: {_sensor.ReadTemperatureOneshot():F2} °F");
                Debug.WriteLine("Altitude is...........: {_sensor.Altitude:F1} meters");
                Thread.Sleep(5000);
            }
        }

        private static void ContinuousMeasurementNoFiFoDemo()
        {
            configuration = new Pressure11Click.SensorConfiguration
            {
                OutputDataRate = Pressure11Click.ODR.OneHertz,
                LowPassFilterConfiguration = Pressure11Click.LowPassFillter.Disabled,
                BlockDataUpdate = Pressure11Click.BDU.Quiescent,

                FiFoEnabled = false,
                StopOnFiFoWatermark = false,
                FiFoWatermarkLevel = 0,
                AutoIncrementRegisterAddress = true,

                PowerModeConfiguration = Pressure11Click.PowerModes.LowNoise
            };

            _sensor.ConfigureSensor(configuration);

            while (true)
            {
                Debug.WriteLine("---Pressure 9 Click Continuous Measurement Demo---");
                Debug.WriteLine("Pressure.................: {_sensor.ReadPressure():F2} mBar");
                Debug.WriteLine("Temperature..............: {_sensor.ReadTemperature():F2} °F");
                Debug.WriteLine("Altitude is..............: {_sensor.Altitude:F1} meters");
            }
        }
    }
}

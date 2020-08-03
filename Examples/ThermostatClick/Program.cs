using MBN;
using MBN.Modules;

using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public class Program
    {
        private static ThermostatClick _sensor;

        public static void Main()
        {
            _sensor = new ThermostatClick(Hardware.SC20100_2, ThermostatClick.I2CAddress.AddressOne)
            {
                TemperatureThreshold = 28.5f,
                TemperatureHysteresis = 26.5f,
                OperatingMode = ThermostatClick.OperatingModes.Comparator,
                Polarity = ThermostatClick.InterruptPolarity.ActiveHigh,
                TemperatureUnit = TemperatureUnits.Celsius
            };

            while (true)
            {
                Debug.WriteLine($"Temperature................: {_sensor.ReadTemperature():F1} °C");
                Thread.Sleep(1000);
            }
        }
    }
}
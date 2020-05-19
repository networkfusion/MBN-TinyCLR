using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public class Program
    {
        private static Thermo4Click _sensor;

        public static void Main()
        {
            _sensor = new Thermo4Click(Hardware.SC20260_2, Thermo4Click.I2cAddress.AddressOne);
            _sensor.TemperatureUnit = TemperatureUnits.Celsius;
            _sensor.PowerMode = PowerModes.On;

            _sensor.SetConfiguration(Thermo4Click.OSMode.Comparator, Thermo4Click.OSPolarity.ActiveLow, Thermo4Click.OSFaultQueue.OneFault);

            while (true)
            {
                Debug.WriteLine($"Temperature : {_sensor.ReadTemperature():F2} °C");
                Thread.Sleep(2000);
            }
        }
    }
}

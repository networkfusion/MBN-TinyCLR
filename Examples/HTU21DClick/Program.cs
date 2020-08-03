using MBN;
using MBN.Modules;

using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static HTU21DClick _sensor;

        public static void Main()
        {
            _sensor = new HTU21DClick(Hardware.SC20100_1)
            {
                MeasurementMode = HTU21DClick.ReadMode.Hold,
                Resolution = HTU21DClick.DeviceResolution.UltraHigh
            };

            while (true)
            {
                Debug.WriteLine("Humidity    " + _sensor.ReadHumidity(HumidityMeasurementModes.Relative).ToString("n2") + " %RH");
                Debug.WriteLine("Temperature " + _sensor.ReadTemperature(TemperatureSources.Ambient).ToString("n2") + " °C");
                Thread.Sleep(1000);
            }
        }
    }
}

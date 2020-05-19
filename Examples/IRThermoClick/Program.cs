using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static IRThermoClick _ir;

        public static void Main()
        {
            _ir = new IRThermoClick(Hardware.SocketThree);

            Debug.WriteLine("Ambient temperature : " + _ir.ReadTemperature().ToString("F2"));
            Debug.WriteLine("Object temperature : " + _ir.ReadTemperature(TemperatureSources.Object).ToString("F2"));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

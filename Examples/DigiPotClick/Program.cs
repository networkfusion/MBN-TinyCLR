using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        static DigiPotClick _dp;

        public static void Main()
        {
            // Initial resistance set to 1K Ohm
            _dp = new DigiPotClick(Hardware.SC20260_2, 1000);

            // Time for measure, it can be removed
            Thread.Sleep(5000);

            Debug.WriteLine("Setting 2K Ohm");
            _dp.Resistance = 2000;
            Debug.WriteLine($"Resistance set to {_dp.Resistance} Ohm");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        static LightClick _lightClick;

        public static void Main()
        {
            _lightClick = new LightClick(Hardware.SC20260_1) { NumberOfSamples = 50 };

            // Sets the range from 0 to 3300 (instead of 0-4095)
            _lightClick.SetScale(0, 3300);

            while (true)
            {
                Debug.WriteLine($"Light(Scaled) : {_lightClick.Read(true)}");
                Debug.WriteLine($"Light(Unscaled) : {_lightClick.Read(false)}");
                Debug.WriteLine($"Light Percent : {_lightClick.ReadLightPercentage()} %");
                Debug.WriteLine($"Light Volts : {_lightClick.ReadVoltage()} V");
                Thread.Sleep(3000);
            }
        }
    }
}

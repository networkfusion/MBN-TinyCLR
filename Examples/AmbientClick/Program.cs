using System.Diagnostics;
using System.Threading;
using MBN;
using MBN.Modules;

namespace Examples
{
    internal class Program
    {
        private static AmbientClick _ambient;

        private static void Main()
        {
            _ambient = new AmbientClick(Hardware.SC20100_2);

            while (true)
            {
                Debug.WriteLine($"Light intensity in  is {_ambient.ReadSensor(10):F0} mW/cm2");
                Thread.Sleep(1000);
            }
        }
    }
}

using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        static ProximityClick _prox;

        public static void Main()
        {
            _prox = new ProximityClick(Hardware.SocketTwo);                   // Proximity at address 0x70 on socket 2

            Debug.WriteLine("Chip revision : " + _prox.ChipRevision);             // Get chip version and firmware revision

            // Set IR Led current to 200 mA  (20 x 10). 
            // Warning : different values of current will cause different readings for the same distance (see datasheet).
            _prox.IRLedCurrent = 20;
            _prox.ProximityRate = 1;     // Set Proximity rate measurement to 3.9 measures/s

            Debug.WriteLine("Ambient light : " + _prox.AmbientLight());           // Get ambient light value

            while (true)
            {
                Debug.WriteLine("Proximity : " + _prox.Distance());               // Get proximity value
                Thread.Sleep(100);
            }
        }
    }
}

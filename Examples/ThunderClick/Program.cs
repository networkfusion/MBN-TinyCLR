using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        static ThunderClick _thunder;

        public static void Main()
        {
            // Create the instance
            _thunder = new ThunderClick(Hardware.SocketOne);

            // Subscribe to events
            _thunder.LightningDetected += TH_LightningDetected;
            _thunder.DisturbanceDetected += TH_DisturbanceDetected;
            _thunder.NoiseDetected += TH_NoiseDetected;

            // Some information
            Debug.WriteLine("Continuous input noise level " + _thunder.ContinuousInputNoiseLevel + " µV rms");

            // Start interrupt scanning
            _thunder.StartIRQ();

            Thread.Sleep(Timeout.Infinite);
        }

        static void TH_NoiseDetected(Object sender, EventArgs e) => Debug.WriteLine("Noise detected");

        static void TH_DisturbanceDetected(Object sender, EventArgs e) => Debug.WriteLine("Disturbance detected");

        static void TH_LightningDetected(Object sender, ThunderClick.LightningEventArgs e) => Debug.WriteLine("Lightning detected at " + e.Distance + " km, energy : " + e.Energy);
    }
}

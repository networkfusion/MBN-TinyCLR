using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    public class Program
    {
        private static AccelClick _accel;

        public static void Main()
        {
            _accel = new AccelClick(Hardware.SocketOne);
            Debug.WriteLine("Device ID : " + _accel.DeviceID);

            // Set the sensor to fixed resolution, updating every 10ms
            _accel.OutputResolution = AccelClick.OutputResolutions.FixedResoultion;
            _accel.UpdateDelay = 10;

            // Single tap/double tap enabled by default, so capture the associated events
            _accel.OnDoubleTap += Accel_OnDoubleTap;
            _accel.OnSingleTap += Accel_OnSingleTap;

            // Start polling
            _accel.Start();

            while (true)
            {
                Thread.Sleep(200);
            }
        }

        private static void Accel_OnSingleTap(Object sender, EventArgs e) => Debug.WriteLine("Single tap");

        private static void Accel_OnDoubleTap(Object sender, EventArgs e) => Debug.WriteLine("Double tap");
    }
}

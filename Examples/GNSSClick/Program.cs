using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static GNSSClick _gnss;

        static void Main()
        {
            TestGNSS();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void TestGNSS()
        {
            _gnss = new GNSSClick(Hardware.SocketOne);

            GPSUtilities.GSAFrameReceived += GPSUtilities_GSAFrameReceived;
            GPSUtilities.GGAFrameReceived += GPSUtilities_GGAFrameReceived;
            GPSUtilities.RMCFrameReceived += GPSUtilities_RMCFrameReceived;
            GPSUtilities.GSVFrameReceived += GPSUtilities_GSVFrameReceived;
            GPSUtilities.L86PMTKFrameReceived += GPSUtilities_L86PMTKFrameReceived;
        }

        private static void GPSUtilities_GSVFrameReceived(Object sender, GPSUtilities.GSVFrameEventArgs e)
        {
            Debug.WriteLine($"{GPSUtilities.FrameCount} - GSV frame received : Signal origin = {e.SignalOrigin}, Satellite[0] = {e.NumberOfMessages}");
        }

        private static void GPSUtilities_GSAFrameReceived(Object sender, GPSUtilities.GSAFrameEventArgs e)
        {
            Debug.WriteLine($"{GPSUtilities.FrameCount} - GSA frame received : Signal origin = {e.SignalOrigin}, 2D/3D mode {e.Auto2D3D}, Fix mode {e.FixMode}");
        }

        private static void GPSUtilities_L86PMTKFrameReceived(Object sender, GPSUtilities.L86PMTKFrameEventArgs e)
        {
            Debug.WriteLine($"{GPSUtilities.FrameCount} - PMTK frame received : PacketType = {e.PacketType}");
        }

        private static void GPSUtilities_GGAFrameReceived(Object sender, GPSUtilities.GGAFrameEventArgs e)
        {
            Debug.WriteLine($"{GPSUtilities.FrameCount} - GGA frame received : Signal origin = {e.SignalOrigin}, Latitude {e.Latitude:F2} {e.LatitudeHemisphere}, Longitude {e.Longitude:F2} {e.LongitudePosition}");
        }

        private static void GPSUtilities_RMCFrameReceived(Object sender, GPSUtilities.RMCFrameEventArgs e)
        {
            Debug.WriteLine($"{GPSUtilities.FrameCount} - RMC frame received : Signal origin = {e.SignalOrigin}, Latitude {e.Latitude:F2} {e.LatitudeHemisphere}, Longitude {e.Longitude:F2} {e.LongitudePosition}");
        }
    }
}

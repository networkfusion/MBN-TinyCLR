using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static UniHallClick _uni;
        static void Main()
        {
            TestUniHall();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void TestUniHall()
        {
            _uni = new UniHallClick(Hardware.SocketOne);
            _uni.MagnetDetected += Uni_MagnetDetected;
        }

        private static void Uni_MagnetDetected(Object sender, UniHallClick.MagnetDetectedEventArgs e) => Debug.WriteLine($"Magnet {(e.MagnetPresent ? "present" : "removed")}");
    }
}

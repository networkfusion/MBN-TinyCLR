using MBN.Modules;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Examples
{
    public partial class Program
    {
        static Int32 _count;

        public static void StartSender()
        {
            _nrf.Configure(Encoding.UTF8.GetBytes("SNDR"), 1, NRFC.DataRate.DR250kbps);
            _nrf.OnTransmitFailed += Nrf_OnTransmitFailed;
            _nrf.OnTransmitSuccess += Nrf_OnTransmitSuccess;
            _nrf.Enable();
            _count = 0;
// ReSharper disable once ObjectCreationAsStatement
            new Timer(SendData, null, new TimeSpan(0, 0, 0, 2), new TimeSpan(0, 0, 0, 2));
        }

        private static void SendData(Object state)
        {
            Thread.Sleep(100);
            _nrf.SendTo(Encoding.UTF8.GetBytes("RCVR"), Encoding.UTF8.GetBytes("Iteration " + _count), NRFC.Acknowledge.No);
            _count++;
        }

        private static void Nrf_OnTransmitSuccess() => Debug.WriteLine("Successfully transmitted text : 'Iteration " + _count + "'");

        private static void Nrf_OnTransmitFailed() => Debug.WriteLine("Transmit failed");
    }
}

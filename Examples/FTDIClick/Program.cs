using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static FTDIClick _ftdi;

        public static void Main()
        {
            _ftdi = new FTDIClick(Hardware.SocketOne);

            _ftdi.DataReceived += Ftdi_DataReceived;
            _ftdi.Listening = true;

            _ftdi.SendData(Encoding.UTF8.GetBytes("Hello world !"));

            Thread.Sleep(Timeout.Infinite);
        }

        static void Ftdi_DataReceived(Object sender, FTDIClick.DataReceivedEventArgs e) => Debug.WriteLine("Data received (" + e.Count + " bytes) : " + new String(Encoding.UTF8.GetChars(e.Data)));
    }
}

using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    internal static class Program
    {
        private static BiHallClick _biHall;

        public static void Main()
        {
            Debug.WriteLine("Started");

            _biHall = new BiHallClick(Hardware.SocketOne);
            _biHall.SwitchStateChanged += BiHallSwitchStateChanged;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void BiHallSwitchStateChanged(Object sender, Boolean switchState) => Debug.WriteLine($"Switch state : { switchState }");
    }
}

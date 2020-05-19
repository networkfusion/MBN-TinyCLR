using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static Storage _fram;
        private static readonly Byte[] Data = new Byte[3];
        public static void Main()
        {
            _fram = new FRAMClick(Hardware.SocketOne);

            Debug.WriteLine("Address 231 before : " + _fram.ReadByte(231));
            _fram.WriteByte(231, 123);
            Debug.WriteLine("Address 231 after : " + _fram.ReadByte(231));

            _fram.WriteData(400, new Byte[] { 100, 101, 102 }, 0, 3);
            _fram.ReadData(400, Data, 0, 3);
            Debug.WriteLine("Read 3 bytes starting @400 (should be 100, 101, 102) : " + Data[0] + ", " + Data[1] + ", " + Data[2]);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

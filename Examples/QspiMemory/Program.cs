using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        public static void Main()
        {
            TestQspiMemory(true);

            Thread.Sleep(Timeout.Infinite);
        }

        private static readonly Byte[] Data = new Byte[3];

        public static void TestQspiMemory(Boolean eraseFirstBlock)
        {
            var qspi = new QspiMemory();

            if (eraseFirstBlock)
                qspi.EraseBlock(0, 1);

            Debug.WriteLine("Address 1 before : " + qspi.ReadByte(1));
            qspi.WriteByte(1, 124);
            Debug.WriteLine("Address 1 after : " + qspi.ReadByte(1));

            qspi.WriteData(10, new Byte[] { 110, 111, 112 }, 0, 3);
            qspi.ReadData(10, Data, 0, 3);
            Debug.WriteLine("Read 3 bytes starting @10 (should be 110, 111, 112) : " + Data[0] + ", " + Data[1] + ", " + Data[2]);
        }
    }
}
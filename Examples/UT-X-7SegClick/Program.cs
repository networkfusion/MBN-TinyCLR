using MBN;
using MBN.Modules;
using System;
using System.Threading;

namespace Examples
{
    class Program
    {
        static void Main()
        {
            Test7Seg();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Test7Seg()
        {
            var seg = new Ut7SegClick(Hardware.SocketOne);

            // Displays from 0 to 9.9
            // Trick : no float here, only bytes, the dot is added as soon as i > 9
            for (Byte i = 0; i < 100; i++)
            {
                seg.Write(i < 10
                    ? new Byte[] { seg.GetDigit(i), 0x00 }
                    : new[] { seg.GetDigit((Byte)(i % 10)), (Byte)(seg.GetDigit((Byte)(i / 10)) | 0b10000000) });
                Thread.Sleep(100);
            }
            Thread.Sleep(2000);

            seg.Clear();
            seg.Enabled = false;
        }
    }
}

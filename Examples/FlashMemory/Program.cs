using GHIElectronics.TinyCLR.Pins;
using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace TestTnyCLR1
{
    class Program
    {
        public static void Main()
        {
            TestFlash();

            Thread.Sleep(Timeout.Infinite);
        }

        private static FlashMemory _flash;
        private static readonly Byte[] Data = new Byte[3];

        public static void TestFlash()
        {
            DoTest(Hardware.SocketOne); // Flash Click on Socket #1
            DoTest(Hardware.SocketTwo); // Flash3 Click on Socket #1
            var onboardFlash = new Hardware.Socket { Cs = STM32H7.GpioPin.PA13, SpiBus = STM32H7.SpiBus.Spi3 };
            DoTest(onboardFlash, false); // Onboard Flash chip on Quail, do not erase block. Results will be depending on the value already present in memory.

            Thread.Sleep(Timeout.Infinite);
        }

        public static void DoTest(Hardware.Socket socket, Boolean eraseFirstBlock = true)
        {
            _flash = new FlashMemory(socket)
            {
                LedIndicator = Hardware.Led2
            };

            if (eraseFirstBlock) _flash.EraseBlock(0, 1);

            Debug.WriteLine("Address 1 before : " + _flash.ReadByte(1));
            _flash.WriteByte(1, 124);
            Debug.WriteLine("Address 1 after : " + _flash.ReadByte(1));

            _flash.WriteData(10, new Byte[] { 110, 111, 112 }, 0, 3);
            _flash.ReadData(10, Data, 0, 3);
            Debug.WriteLine("Read 3 bytes starting @10 (should be 110, 111, 112) : " + Data[0] + ", " + Data[1] + ", " + Data[2]);
        }
    }
}

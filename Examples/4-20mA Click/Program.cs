using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static T4_20mAClick _transmitter;
        private static R4_20mAClick _receiver;
        static void Main()
        {
            Test4_20();

            Thread.Sleep(Timeout.Infinite);
        }

        // The calibration data used for this test is not the one that will be used in real applications.
        // The test rig for this example only consisted in the transmitter+receiver connected on the same board with a short wire. There waq no device on the loop.
        // Once you add a real device on the loop, you should calibrate the transmitter with a precise current-meter :
        //  - Send some unscaled data (generally lower than 850) via the transmitter
        //  - Read the value on the current meter
        //  - Change the value until you read exactly 4mA
        //  - Repeat this process for the 20mA value
        //  - Set those calibration values using the SetCalibrationData() method
        //  - Use them in the Receiver constructor
        // Common values for a simple loop are around 792 for 4mA and 3960 for 20mA

        private static void Test4_20()
        {
            _transmitter = new T4_20mAClick(Hardware.SocketOne);
            _transmitter.SetCalibrationData(20, 4000);
            new Thread(ThreadTransmit).Start();

            _receiver = new R4_20mAClick(Hardware.SocketFour, 20, 4000);
            new Thread(ThreadReceive).Start();
        }

        private static void ThreadTransmit()
        {
            while (true)
            {
                _transmitter.WriteDAC(100);
                Thread.Sleep(3000);
                _transmitter.WriteDAC(400);
                Thread.Sleep(3000);
                _transmitter.WriteDAC(800);
                Thread.Sleep(3000);
                _transmitter.WriteDAC(1000);
                Thread.Sleep(3000);
            }
        }

        private static void ThreadReceive()
        {
            UInt16 value;

            while (true)
            {
                value = _receiver.ReadDAC(10);
                Debug.WriteLine($"Value read : {value}");
                Thread.Sleep(1000);
            }
        }
    }
}

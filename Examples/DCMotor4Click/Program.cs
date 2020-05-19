using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static DCMotor4Click _dc;

        static void Main()
        {
            TestDC4();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void TestDC4()
        {
            _dc = new DCMotor4Click(Hardware.SocketOne);

            Debug.WriteLine("Moving Forward");
            _dc.Move(DCMotor4Click.Directions.Forward);
            Thread.Sleep(2500);
            _dc.Stop();
            Thread.Sleep(200);
            Debug.WriteLine("Moving Backward");
            _dc.Move(DCMotor4Click.Directions.Backward);
            Thread.Sleep(2500);
            _dc.Stop();
            Thread.Sleep(200);
            Debug.WriteLine("Moving Forward ramping during 5s and stay 10s rotating");
            _dc.Move(DCMotor4Click.Directions.Forward, 1.0, 5000);
            Thread.Sleep(10000);
            Debug.WriteLine("Stopping motor in 5s, ramping down speed");
            _dc.Stop(5000);
        }
    }
}

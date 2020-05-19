using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        static DCMotorClick _motor;

        public static void Main()
        {
            _motor = new DCMotorClick(Hardware.SocketOne);

            _motor.OnFault += Motor_OnFault;

            _motor.Move(DCMotorClick.Directions.Forward, 1.0, 2000);
            Thread.Sleep(5000);
            _motor.Stop(2000);

            while (_motor.IsMoving) { Thread.Sleep(10); }

            Debug.WriteLine("Sleep");
            _motor.PowerMode = PowerModes.Low;

            Debug.WriteLine("Move");
            _motor.Move(DCMotorClick.Directions.Backward, 0.75);
            Thread.Sleep(2000);
            _motor.Stop();


            Debug.WriteLine("Wake up");
            _motor.PowerMode = PowerModes.On;

            Debug.WriteLine("Move");
            _motor.Move(DCMotorClick.Directions.Backward);
            Thread.Sleep(3000);
            _motor.Stop();
        }

        static void Motor_OnFault(Object sender, EventArgs e) => Debug.WriteLine("Over current protection !!!");
    }
}

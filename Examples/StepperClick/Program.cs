using MBN;
using MBN.Modules;
using System.Threading;

namespace Examples
{
    public class Program
    {
        private static StepperClick _stepper;

        static void Main()
        {
            _stepper = new StepperClick(Hardware.SocketTwo);

            for (var i = 0; i < 400; i++)
            {
                _stepper.Step();
                Thread.Sleep(3);
            }
            _stepper.Direction = StepperClick.Directions.CounterClockwise;
            for (var i = 0; i < 400; i++)
            {
                _stepper.Step();
                Thread.Sleep(3);
            }
            _stepper.StepSize = StepperClick.StepSizes.Half;
            for (var i = 0; i < 400; i++)
            {
                _stepper.Step();
                Thread.Sleep(3);
            }

            _stepper.StepSize = StepperClick.StepSizes.Fourth;
            for (var i = 0; i < 400; i++)
            {
                _stepper.Step();
                Thread.Sleep(3);
            }

            _stepper.StepSize = StepperClick.StepSizes.Eighth;
            for (var i = 0; i < 400; i++)
            {
                _stepper.Step();
                Thread.Sleep(3);
            }

            _stepper.StepSize = StepperClick.StepSizes.Full;
            _stepper.Direction = StepperClick.Directions.Clockwise;
            for (var i = 0; i < 400; i++)
            {
                _stepper.Step();
                Thread.Sleep(3);
            }
            _stepper.Enabled = false;

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

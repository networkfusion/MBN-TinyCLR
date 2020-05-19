using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static JoystickClick _joy;

        public static void Main()
        {
            _joy = new JoystickClick(Hardware.SC20260_1) { DeadZone = new SByte[] { 100, -100, 100, -100 } };

            Debug.WriteLine($"DZ = {_joy.DeadZone[0]}, {_joy.DeadZone[1]}, {_joy.DeadZone[2]}, {_joy.DeadZone[3]}");
            _joy.TimeBase = 7;

            _joy.InterruptLine.ValueChanged += InterruptLine_ValueChanged;
            _joy.Button.ValueChanged += Button_ValueChanged;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Button_ValueChanged(GHIElectronics.TinyCLR.Devices.Gpio.GpioPin sender, GHIElectronics.TinyCLR.Devices.Gpio.GpioPinValueChangedEventArgs e) => Debug.WriteLine($"Edge : {e.Edge}");

        private static void InterruptLine_ValueChanged(GHIElectronics.TinyCLR.Devices.Gpio.GpioPin sender, GHIElectronics.TinyCLR.Devices.Gpio.GpioPinValueChangedEventArgs e)
        {
            JoystickClick.KnobPosition pos = _joy.GetKnobPosition();
            Debug.WriteLine($"Interrupt (X,Y) : {pos.X}, {pos.Y}");
        }
    }
}

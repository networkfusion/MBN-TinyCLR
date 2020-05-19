using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        public static I2CMuxClick _mux;
        static void Main()
        {
            DemoMux1();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void DemoMux1()
        {
            // In this demo, we have 4 Proximity Click modules connected to the I2CMux Click
            var _sensors = new ProximityClick[4];
            _mux = new I2CMuxClick(Hardware.SocketOne, 0xE0 >> 1, 100000);

            // Note that the four sensors are on the same socket and have the same I2C address
            // Since the Quail has only one I2C bus, it's not really meaningful here. But it will be on the Ram board, which has two I2C buses.
            _sensors[0] = new ProximityClick(Hardware.SocketOne);
            _sensors[1] = new ProximityClick(Hardware.SocketOne);
            _sensors[2] = new ProximityClick(Hardware.SocketOne);
            _sensors[3] = new ProximityClick(Hardware.SocketOne);

            for (var i = 0; i < 4; i++)
            {
                // Multiplexer channel will be : 0b00000001, then 0b00000010, then 0b00000100 and finally 0b00001000
                // This will activate each Proximity Click on its channel on the I2CMux Click
                _mux.ActiveChannels = (Byte)(1 << i);
                Debug.WriteLine($"Proximity Click on channel {i} reads a distance of {_sensors[i].Distance()} m");
            }
        }

        private static void DemoMux2()
        {
            // In this demo, one sensor is on the I2CMux Click (which is on Quail's socket #1) on channel 0 and the other sensor on Quail's socket #2
            _mux = new I2CMuxClick(Hardware.SocketOne, 0xE0 >> 1, 100000);

            var _prox1 = new ProximityClick(Hardware.SocketOne);
            var _prox2 = new ProximityClick(Hardware.SocketTwo);

            // Activate reading for the first sensor
            _mux.ActiveChannels = 0b00000001;
            Debug.WriteLine($"Proximity Click on socket #1 reads a distance of {_prox1.Distance()} m");

            // Deactivate all channels on the I2CMux Click so that the command will not be listened
            // This will allow the I2C command to be directed to the sensor on socket #2
            _mux.ActiveChannels = 0b00000000;
            Debug.WriteLine($"Proximity Click on socket #2 reads a distance of {_prox2.Distance()} m");
        }
    }
}

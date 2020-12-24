#if (NANOFRAMEWORK_1_0)
using System.Device.Gpio;
using Windows.Devices.Uart;
#else
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Uart;
#endif

using System;
using System.Text;
using System.Threading;
using static MBN.Modules.GPSUtilities;

namespace MBN.Modules
{
    /// <summary>Main GPS2 Click class</summary>
    public sealed partial class GPS2Click
    {
        private readonly UartController _gps2;
        private readonly SerialListener _sl;
        private readonly GpioPin _wakeUp, _onOff;

        /// <summary>Initializes a new instance of the <see cref="GPS2Click" /> class.</summary>
        /// <param name="socket">The socket on which the module is plugged</param>
        public GPS2Click(Hardware.Socket socket)
        {
            _sl = new SerialListener('$', '\n');
            _sl.MessageAvailable += Sl_MessageAvailable;

            _wakeUp = GpioController.GetDefault().OpenPin(socket.AnPin);
            _wakeUp.SetDriveMode(GpioPinDriveMode.Input);

            _onOff = GpioController.GetDefault().OpenPin(socket.PwmPin);
            _onOff.SetDriveMode(GpioPinDriveMode.Output);
            // Force full mode
            _onOff.Write(GpioPinValue.Low);
            Thread.Sleep(100);
            _onOff.Write(GpioPinValue.High);
            Thread.Sleep(100);
            _onOff.Write(GpioPinValue.Low);
            Thread.Sleep(1);
            _onOff.Write(GpioPinValue.High);
            Thread.Sleep(100);

            _gps2 = UartController.FromName(socket.ComPort); // Socket #1
            _gps2.SetActiveSettings(new UartSetting() { BaudRate = 4800, DataBits = 8, Parity = UartParity.None, StopBits = UartStopBitCount.One, Handshaking = UartHandshake.None });
            _gps2.DataReceived += Gps2_DataReceived;
            _gps2.Enable();
        }

        private void Sl_MessageAvailable(Object sender, EventArgs e)
        {
            var param = (Byte[])_sl.MessagesQueue.Dequeue();
            var _chars = new Char[param.Length];

            Encoding.UTF8.GetDecoder().Convert(param, 0, param.Length, _chars, 0, param.Length, false, out _, out var _charsUsed, out _);
            var strtmp = new String(_chars, 0, _charsUsed).Trim('\r', '\n');

            if (strtmp != String.Empty)
                Parse(strtmp);
        }

        /// <summary>Sends a command to the GPS2 module.</summary>
        /// <param name="cmd">The command, without both the starting '$' and the ending '*'.</param>
        public void SendCommand(String cmd)
        {
            _gps2.Write(Encoding.UTF8.GetBytes($"${cmd}*{CalculateChecksum(cmd):X2}\r\n"));
            _gps2.Flush();
        }

        private void Gps2_DataReceived(UartController sender, DataReceivedEventArgs e)
        {
            var btr = _gps2.BytesToRead;
            while (btr != 0)
            {
                var _buffer = new Byte[btr];
                _gps2.Read(_buffer, 0, btr);
                _sl.Add(_buffer);
                _buffer = null;
                btr = _gps2.BytesToRead;
            }
        }
    }
}

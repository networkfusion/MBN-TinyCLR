#if (NANOFRAMEWORK_1_0)
#if USE_I2C
using System.Device.I2c;
#else
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
#endif
#else
using GHIElectronics.TinyCLR.Devices.Uart;
#endif

using System;
using System.Text;
using static MBN.Modules.GPSUtilities;

namespace MBN.Modules
{
    /// <summary>Main GNSS 5 Click class</summary>
    public sealed partial class Gnss5Click
    {
#if (NANOFRAMEWORK_1_0)
#if USE_I2C
        private readonly I2cDevice _gnss;
#else
        private readonly SerialDevice _gnss;
#endif
#else
        private readonly UartController _gnss;
#endif

        private readonly SerialListener _sl;

        /// <summary>Initializes a new instance of the <see cref="Gnss5Click" /> class.</summary>
        /// <param name="socket">The socket on which the module is plugged</param>
        public Gnss5Click(Hardware.Socket socket)
        {
            _sl = new SerialListener('$', '\n');
            _sl.MessageAvailable += Sl_MessageAvailable;

#if (NANOFRAMEWORK_1_0)
#if USE_I2C
            // store I2C address
            //_address = address;
            var address = 0x42;

            // instantiate I2C controller
            _gnss = I2cDevice.Create(new I2cConnectionSettings(socket.I2cBus, address, I2cBusSpeed.FastMode));
#else
            _gnss = SerialDevice.FromId(socket.ComPort);

            // set parameters
            _gnss.BaudRate = 9600;
            _gnss.Parity = SerialParity.None;
            _gnss.StopBits = SerialStopBitCount.One;
            _gnss.Handshake = SerialHandshake.None;
            _gnss.DataBits = 8;
#endif
#else
            _gnss = UartController.FromName(socket.ComPort);
            _gnss.SetActiveSettings(new UartSetting() { BaudRate = 9600, DataBits = 8, Parity = UartParity.None, StopBits = UartStopBitCount.One, Handshaking = UartHandshake.None });
#endif
            _gnss.DataReceived += Gnss_DataReceived;

#if (!NANOFRAMEWORK_1_0)
            _gnss.Enable();
#endif
        }

        private void Sl_MessageAvailable(object sender, EventArgs e)
        {
            var param = (byte[])_sl.MessagesQueue.Dequeue();
            var _chars = new char[param.Length];

            Encoding.UTF8.GetDecoder().Convert(param, 0, param.Length, _chars, 0, param.Length, false, out _, out var _charsUsed, out _);
            var strtmp = new string(_chars, 0, _charsUsed).Trim('\r', '\n');

            if (strtmp != String.Empty)
                Parse(strtmp);
        }

        /// <summary>Sends a command to the GNSS 5 module.</summary>
        /// <param name="cmd">The command, without both the starting '$' and the ending '*'.</param>
        public void SendCommand(string cmd)
        {
#if (NANOFRAMEWORK_1_0)
            DataWriter outputDataWriter = new DataWriter(_gnss.OutputStream);
            var bytesToWrite = Encoding.UTF8.GetBytes($"${cmd}*{CalculateChecksum(cmd):X2}\r\n");
            outputDataWriter.WriteBytes(bytesToWrite);
            outputDataWriter.Store();
#else
            _gnss.Write(Encoding.UTF8.GetBytes($"${cmd}*{CalculateChecksum(cmd):X2}\r\n"));
            _gnss.Flush();
#endif
        }

#if (NANOFRAMEWORK_1_0)
        private void Gnss_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DataReader inputDataReader = new DataReader(_gnss.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial
            };
#else
        private void Gnss_DataReceived(UartController sender, DataReceivedEventArgs e)
        {
#endif
            var btr = _gnss.BytesToRead;
            while (btr != 0)
            {
                var _buffer = new Byte[btr];
#if (NANOFRAMEWORK_1_0)
                inputDataReader.Load(btr);
                inputDataReader.ReadBytes(_buffer);
#else
                _gnss.Read(_buffer, 0, btr);
#endif
                _sl.Add(_buffer);
                _buffer = null;
                btr = _gnss.BytesToRead;
            }
        }
    }
}

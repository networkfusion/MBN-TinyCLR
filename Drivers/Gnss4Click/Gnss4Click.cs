#if (NANOFRAMEWORK_1_0)
using Windows.Devices.Uart;
#else
using GHIElectronics.TinyCLR.Devices.Uart;
#endif

using System;
using System.Text;

namespace MBN.Modules
{
    /// <summary>Main GNSS 4 Click class</summary>
    public sealed partial class Gnss4Click
    {
        private readonly UartController _gnss;
        private readonly SerialListener _sl;

        /// <summary>Initializes a new instance of the <see cref="Gnss4Click" /> class.</summary>
        /// <param name="socket">The socket on which the module is plugged</param>
        public Gnss4Click(Hardware.Socket socket)
        {
            _sl = new SerialListener('$', '\n');
            _sl.MessageAvailable += Sl_MessageAvailable;

            _gnss = UartController.FromName(socket.ComPort);
            _gnss.SetActiveSettings(new UartSetting() { BaudRate = 9600, DataBits = 8, Parity = UartParity.None, StopBits = UartStopBitCount.One, Handshaking = UartHandshake.None });
            _gnss.DataReceived += Gnss_DataReceived;
            _gnss.Enable();
        }

        private void Sl_MessageAvailable(Object sender, EventArgs e) => NMEAParser.Parse((Byte[])_sl.MessagesQueue.Dequeue());

        /// <summary>Sends a command to the GNSS 4 module.</summary>
        /// <param name="cmd">The command, with both the starting '$' and the ending '*'.</param>
        public void SendCommand(String cmd)
        {
            _gnss.Write(Encoding.UTF8.GetBytes($"{cmd}{NMEAParser.CalculateChecksum(Encoding.UTF8.GetBytes(cmd)):X2}\r\n"));
            _gnss.Flush();
        }

        private void Gnss_DataReceived(UartController sender, DataReceivedEventArgs e)
        {
            var btr = _gnss.BytesToRead;
            while (btr != 0)
            {
                var _buffer = new Byte[btr];
                _gnss.Read(_buffer, 0, btr);
                _sl.Add(_buffer);
                _buffer = null;
                btr = _gnss.BytesToRead;
            }
        }
    }
}

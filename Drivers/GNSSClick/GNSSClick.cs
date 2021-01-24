using GHIElectronics.TinyCLR.Devices.Uart;
using System;
using System.Text;

namespace MBN.Modules
{
    /// <summary>Main GNSS Click class</summary>
    public sealed partial class GNSSClick
    {
        private readonly UartController _gnss;
        private readonly SerialListener _sl;

        /// <summary>Initializes a new instance of the <see cref="GNSSClick" /> class.</summary>
        /// <param name="socket">The socket on which the module is plugged</param>
        public GNSSClick(Hardware.Socket socket)
        {
            _sl = new SerialListener('$', '\n');
            _sl.MessageAvailable += Sl_MessageAvailable;

            _gnss = UartController.FromName(socket.ComPort);
            _gnss.SetActiveSettings(new UartSetting() { BaudRate = 9600, DataBits = 8, Parity = UartParity.None, StopBits = UartStopBitCount.One, Handshaking = UartHandshake.None });
            _gnss.DataReceived += Gnss_DataReceived;
            _gnss.Enable();
            // It seems that we need to send a command so that the module starts sending its data.
            // So we send a command to request firmware version
            SendCommand("PMTK605");
        }

        private void Sl_MessageAvailable(Object sender, EventArgs e) => NMEAParser.Parse((Byte[])_sl.MessagesQueue.Dequeue());

        /// <summary>Sends a command to the GNSS module.</summary>
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

/*
 * FTDIClick driver for TinyCLR 2.0
 * 
 * Initial revision
 *  
 * Copyright 2020 MikroBus.Net
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. See the License for the specific language governing permissions and limitations under the License.
 * 
 */


#if (NANOFRAMEWORK_1_0)
using Windows.Devices.SerialCommunication;
#else
using GHIElectronics.TinyCLR.Devices.Uart;
#endif

using System;

namespace MBN.Modules
{
    /// <summary>
    /// Main class for the FTDIClick driver
    /// <para><b>Pins used :</b> Tx, Rx</para>
    /// </summary>
    /// <example>
    /// <code language="C#">
    /// using System;
    /// using System.Text;
    /// using MBN.Modules;
    /// using MBN;
    /// using MBN.Exceptions;
    /// using System.Threading;
    /// using Microsoft.SPOT;
    /// 
    /// namespace Examples
    /// {
    ///     public class Program
    ///     {
    ///         private static FTDIClick _ftdi;
    /// 
    ///         public static void Main()
    ///         {
    ///             try
    ///             {
    ///                 _ftdi = new FTDIClick(Hardware.SocketOne);
    /// 
    ///                 _ftdi.DataReceived += _ftdi_DataReceived;
    ///                 _ftdi.Listening = true;
    /// 
    ///                 _ftdi.SendData(Encoding.UTF8.GetBytes("Hello world !"));
    ///             }
    ///             catch (PinInUseException)
    ///             {
    ///                 Debug.Print("Error accessing the FTDI Click board");
    ///             }
    /// 
    ///             Thread.Sleep(Timeout.Infinite);
    ///         }
    /// 
    ///         static void _ftdi_DataReceived(object sender, FTDIClick.DataReceivedEventArgs e)
    ///         {
    ///             Debug.Print("Data received (" + e.Count + " bytes) : " + new String(Encoding.UTF8.GetChars(e.Data)));
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
// ReSharper disable once InconsistentNaming
    public sealed partial class FTDIClick
    {
        // This is a demo event fired by the driver. Implementation is in the Christophe.Events namespace below.
        /// <summary>
        /// Occurs when a demo event is detected.
        /// </summary>
        /// <example>
        /// <code language="C#">
        ///         public static void Main()
        ///         {
        ///             try
        ///             {
        ///                 _ftdi = new FTDIClick(Hardware.SocketOne);
        /// 
        ///                 _ftdi.DataReceived += _ftdi_DataReceived;
        ///                 _ftdi.Listening = true;
        /// 
        ///                 _ftdi.SendData(Encoding.UTF8.GetBytes("Hello world !"));
        ///             }
        ///             catch (PinInUseException)
        ///             {
        ///                 Debug.Print("Error accessing the FTDI Click board");
        ///             }
        /// 
        ///             Thread.Sleep(Timeout.Infinite);
        ///         }
        /// 
        ///         static void _ftdi_DataReceived(object sender, FTDIClick.DataReceivedEventArgs e)
        ///         {
        ///             Debug.Print("Data received (" + e.Count + " bytes) : " + new String(Encoding.UTF8.GetChars(e.Data)));
        ///         }
        /// </code>
        /// </example>
        public event DataReceivedEventHandler DataReceived = delegate { };

        private readonly UartController _sp;
        private Boolean _listening;

        /// <summary>
        /// Initializes a new instance of the <see cref="FTDIClick" /> class.
        /// </summary>
        /// <param name="socket">The socket on which the FTDIClick module is plugged on MikroBus.Net board</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">The parity.</param>
        /// <param name="dataBits">The data bits.</param>
        /// <param name="stopBits">The stop bits.</param>
        public FTDIClick(Hardware.Socket socket, Int32 baudRate = 9600, UartParity parity = UartParity.None, Int32 dataBits = 8, UartStopBitCount stopBits = UartStopBitCount.One)
        {
            _sp = UartController.FromName(socket.ComPort);
            _sp.SetActiveSettings(new UartSetting() { BaudRate = baudRate, DataBits = dataBits, Parity = parity, StopBits = stopBits, Handshaking = UartHandshake.None });
            _sp.Enable();
        }

        private void Sp_DataReceived(UartController sender, GHIElectronics.TinyCLR.Devices.Uart.DataReceivedEventArgs e)
        {
            var nb = _sp.BytesToRead;
            var buf = new Byte[nb];

            _sp.Read(buf, 0, nb);
            DataReceivedEventHandler tempEvent = DataReceived;
            tempEvent(this, new DataReceivedEventArgs(buf, nb));
        }

        /// <summary>
        /// Sends data to the FTDI chip.
        /// </summary>
        /// <param name="data">Array of bytes containing data to send</param>
        /// <example>
        /// <code language="C#">
        ///         public static void Main()
        ///         {
        ///             try
        ///             {
        ///                 _ftdi = new FTDIClick(Hardware.SocketOne);
        /// 
        ///                 _ftdi.SendData(Encoding.UTF8.GetBytes("Hello world !"));
        ///             }
        ///             catch (PinInUseException)
        ///             {
        ///                 Debug.Print("Error accessing the FTDI Click board");
        ///             }
        /// 
        ///             Thread.Sleep(Timeout.Infinite);
        ///         }
        /// </code>
        /// </example>
        public void SendData(Byte[] data) => _sp.Write(data, 0, data.Length);

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FTDIClick"/> is listening on the serial port
        /// </summary>
        /// <value>
        ///   <c>true</c> if listening; otherwise, <c>false</c>.
        /// </value>
        /// <example>
        /// <code language="C#">
        ///         public static void Main()
        ///         {
        ///             try
        ///             {
        ///                 _ftdi = new FTDIClick(Hardware.SocketOne);
        /// 
        ///                 _ftdi.DataReceived += _ftdi_DataReceived;
        ///                 _ftdi.Listening = true;
        /// 
        ///             }
        ///             catch (PinInUseException)
        ///             {
        ///                 Debug.Print("Error accessing the FTDI Click board");
        ///             }
        /// 
        ///             Thread.Sleep(Timeout.Infinite);
        ///         }
        /// 
        ///         static void _ftdi_DataReceived(object sender, FTDIClick.DataReceivedEventArgs e)
        ///         {
        ///             Debug.Print("Data received (" + e.Count + " bytes) : " + new String(Encoding.UTF8.GetChars(e.Data)));
        ///         }
        /// </code>
        /// </example>
        public Boolean Listening
        {
            get { return _listening; }
            set
            {
                if (value == _listening) { return; }
                if (value) { _sp.DataReceived += Sp_DataReceived; }
                else { _sp.DataReceived -= Sp_DataReceived; }
                _listening = value;
            }
        }
    }
}


#region Usings

using MBN;
using MBN.Modules;

using System;
using System.Diagnostics;
using System.Threading;

#endregion

namespace Examples
{
    internal class Program
    {
        private static void Main()
        {
            // Relay Click board is plugged on socket #1 of the MikroBus.Net mainboard
            // Relay 0 will be OFF and Relay 1 will be ON at startup
            RelayClick _relays = new RelayClick(Hardware.SocketOne, relay1InitialState: true);

            // Register to the event generated when a relay state has been changed
            _relays.RelayStateChanged += Relays_RelayStateChanged;

            // Sets relay 0 to ON using the SetRelay() method
            _relays.SetRelay(0, true);
            Thread.Sleep(2000);

            // Sets relay 0 to OFF using the Relay0 property
            _relays.Relay0 = false;
            Thread.Sleep(2000);

            // Sets relay 1 to ON using the SetRelay() method
            _relays.SetRelay(1, true);
            Thread.Sleep(2000);

            // Sets relay 1 to OFF using the Relay0 property
            _relays.Relay1 = false;
            Thread.Sleep(2000);

            // Gets relay 0 state using the Relay0 property
            Debug.WriteLine("Relay 0 state : " + _relays.Relay0);

            // Gets relay 1 state using the Relay0 property
            Debug.WriteLine("Relay 1 state : " + _relays.Relay1);

            // Gets relay 1 state using the GetRelay() method
            Thread.Sleep(Timeout.Infinite);

            Debug.WriteLine("Relay 1 state : " + _relays.GetRelay(1));
        }

        private static void Relays_RelayStateChanged(Object sender, RelayStateChangedEventArgs e)
        {
            Debug.WriteLine(e.Relay == 0 ? "Relay 0 state has changed" : "Relay 1 state has changed");
        }
    }
}
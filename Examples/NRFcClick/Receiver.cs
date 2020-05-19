using MBN.Modules;
using System.Diagnostics;
using System.Text;

namespace Examples
{
    public partial class Program
    {
        public static void StartReceiver()
        {
            _nrf.Configure(Encoding.UTF8.GetBytes("RCVR"), 1, NRFC.DataRate.DR250kbps);
            _nrf.OnDataReceived += Nrf_OnDataReceived;
            _nrf.Enable();
        }

        private static void Nrf_OnDataReceived(System.Byte[] data) => Debug.WriteLine("Received : " + new System.String(Encoding.UTF8.GetChars(data)));
    }
}

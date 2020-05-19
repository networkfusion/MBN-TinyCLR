using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static SpeakUpClick _speakUp;

        public static void Main()
        {
            _speakUp = new SpeakUpClick(Hardware.SocketTwo);

            _speakUp.SpeakDetected += SpeakUp_SpeakDetected;
            _speakUp.Listening = true;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void SpeakUp_SpeakDetected(System.Object sender, SpeakUpClick.SpeakUpEventArgs e) => Debug.WriteLine("SpeakUp has detected an order, index : " + e.Command);
    }
}

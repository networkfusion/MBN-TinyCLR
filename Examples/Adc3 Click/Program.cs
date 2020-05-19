using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static Adc3Click _adc;
        static void Main()
        {
            TestAdc3();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void TestAdc3()
        {
            _adc = new Adc3Click(Hardware.SocketOne)
            {
                Channel = Adc3Click.Channels.Channel2,
                SampleRate = Adc3Click.SampleRates._14bits,
                Gain = Adc3Click.GainSelection.x2
            };

            while (true)
            {
                Debug.WriteLine($"Voltage = {_adc.GetVoltage():F2} V");
                Thread.Sleep(1000);
            }
        }
    }
}

using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static FMClick _fm;

        public static void Main()
        {
            /* If you are outside the USA or Australia, you must change the Channel Spacing and Radio Band from the default of Spacing.UsaAustralia and Band.UsaEurope
                 *  In Europe - use Spacing.EuropeJapan and Band.UsaEurope.
                 *  In Japan - use Spacing.EuropeJapan and Band.Japan or JapanWide.
                 *  No other configurations are available.

                    if (_fm.ChannelSpacing != FMClick.Spacing.UsaAustralia || _fm.RadioBand != FMClick.Band.UsaEurope)
                    {
                        _fm.SetRadioConfiguration(FMClick.Spacing.EuropeJapan, FMClick.Band.UsaEurope);
                    }
                 */
            _fm = new FMClick(Hardware.SocketOne)
            {
                Volume = 7,
                Station = 93.3
            };

            _fm.RadioTextChanged += FM_RadioTextChanged;

            new Thread(Capture).Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void FM_RadioTextChanged(FMClick sender, System.String newradiotext) => Debug.WriteLine("RDS Text received - " + newradiotext);

        private static void Capture()
        {
            while (true)
            {
                var rssi = _fm.RSSI;
                var rssiPercentage = (System.Double)rssi / 75 * 100;

                Debug.WriteLine("RSSI: " + _fm.RSSI + " of 75 (" + rssiPercentage.ToString("F0") + "%)");
                Debug.WriteLine("Stereo: " + _fm.IsStereo);
                Debug.WriteLine("Is Muted ? " + _fm.Mute);
                Debug.WriteLine("ChannelSpacing? " + _fm.ChannelSpacing);
                Debug.WriteLine("RadioBand? " + _fm.RadioBand);
                Debug.WriteLine("Station ? " + _fm.Station.ToString("D1") + " Hz");
                Debug.WriteLine("RDS: " + _fm.RadioText + "\n");
                Thread.Sleep(3000);
            }
        }
    }
}

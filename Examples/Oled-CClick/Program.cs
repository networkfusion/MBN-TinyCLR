using MBN;
using MBN.Modules;

using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

using System.Threading;

namespace Examples
{
    public class Program
    {
        static OLEDCClick _oled;
        static readonly MikroFont _font1 = FontManager.GetFont(FontManager.FontName.TahomaReg12);

        public static void Main()
        {
            _oled = new OLEDCClick(Hardware.SocketOne) { FrameRate = OLEDCClick.FrameRates.OCS_140_4Hz };

            _oled.Canvas.Clear(KnownColors.Wheat);
            _oled.Canvas.DrawText("Hello", _font1, KnownColors.Red, 0, (_oled.CanvasHeight - _font1.FontHeight) / 2, 96, _font1.FontHeight, true);
            _oled.Flush();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

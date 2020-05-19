using MBN;
using MBN.Modules;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static TinyFileSystem _tfs;

        public static void Main()
        {
            _tfs = new TinyFileSystem(new FlashMemory(Hardware.OnboardFlash));
            if (_tfs.CheckIfFormatted())
            {
                Debug.WriteLine("Filesystem OK. Mounting...");
                _tfs.Mount();
                Debug.WriteLine("Mounted. Now reading settings.dat file...");
                if (!_tfs.Exists("settings.dat")) return;
                using (Stream fs = _tfs.Open("settings.dat", FileMode.Open))
                using (var rdr = new StreamReader(fs))
                {
                    System.String line;
                    while ((line = rdr.ReadLine()) != null)
                    {
                        Debug.WriteLine(line);
                    }
                }
                TinyFileSystem.DeviceStats aa = _tfs.GetStats();
                Debug.WriteLine("Stats : " + aa);
            }
            else
            {
                Debug.WriteLine("Formatting");
                _tfs.Format();
                Debug.WriteLine("Creating file");
                using (Stream fs = _tfs.Create("settings.dat"))
                {
                    using (var wr = new StreamWriter(fs))
                    {
                        wr.WriteLine("<settings>");
                        wr.WriteLine("InitialPosX=200");
                        wr.WriteLine("InitialPosY=150");
                        wr.WriteLine("</settings>");
                        wr.Flush();
                        fs.Flush();
                    }
                }
                Debug.WriteLine("FileCreated");
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

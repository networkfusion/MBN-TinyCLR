using MBN;

using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using MBN.Modules;

namespace Examples
{
    public class Program
    {
        private static UniqueIDClick _uniqueIdClick;

        public static void Main()
        {
            _uniqueIdClick = new UniqueIDClick(Hardware.SC20100_2, UniqueIDClick.GpioSelect.GP0);

            ArrayList devices = new ArrayList();

            foreach (var device in _uniqueIdClick.DeviceList)
            {
                devices.Add(_uniqueIdClick.GetSerialNumber((Byte[])device));
            }

            foreach (var i in devices)
            {
                Debug.WriteLine($"Device with Serial Number {i} has One-Wire Address of {GetDeviceId(_uniqueIdClick.SerialNumberToOneWireAddress((Int64)i))}");
            }

            Thread.Sleep(Timeout.Infinite);
        }

        private static String GetDeviceId(Byte[] id)
        {
            return $"[0x{id[0]:x2}, 0x{id[1]:x2}, 0x{id[2]:x2}, 0x{id[3]:x2}, 0x{id[4]:x2}, 0x{id[5]:x2}, 0x{id[6]:x2}, 0x{id[7]:x2}]";
        }
    }
}

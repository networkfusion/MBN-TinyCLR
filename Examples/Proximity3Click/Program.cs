using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static Proximity3Click _prox3;
        private static Boolean _runProximity3;

        static void Main()
        {
            TestProximity3();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void TestProximity3()
        {
            _prox3 = new Proximity3Click(Hardware.SocketFour)
            {
                IntegrationTime = Proximity3Click.IntegrationTimes._200ms
            };
            Debug.WriteLine($"Chip revision : {_prox3.ChipRevision}");

            _runProximity3 = true;
            new Thread(ThreadProximity3).Start();
        }

        private static void ThreadProximity3()
        {
            var i = 0;
            while (_runProximity3)
            {
                Debug.WriteLine($"Proximity3 measure {i}, distance = {_prox3.Distance}, ambient light = {_prox3.AmbientLight}");

                Thread.Sleep(250);
                i++;
            }
        }
    }
}

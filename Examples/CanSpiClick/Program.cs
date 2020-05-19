using GHIElectronics.TinyCLR.Devices.Can;
using GHIElectronics.TinyCLR.Pins;
using MBN;
using MBN.Modules;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Examples
{
    class Program
    {
        private static CanSpiClick _canSpi;
        private static CanController _onboardCan;

        static void Main()
        {
            TestCanSpi();

            Thread.Sleep(Timeout.Infinite);
        }

        public static void TestCanSpi()
        {
            // Initialize CanSpi Click module on SC20260D socket #2
            _canSpi = new CanSpiClick(Hardware.SC20260_2);
            if (_canSpi.Init("CAN#1", CanSpiClick.Baudrate500k, CanSpiClick.normalMode))
                Debug.WriteLine("CAN#1 @ 500kbps");
            else
                throw new NotImplementedException("CanSpiClick initialization failed!");
            _canSpi.MessageReceived += CAN1_MessageReceived;

            // Initialize SC20260D onboard Can
            _onboardCan = CanController.FromName(STM32H7.CanBus.Can1);
            _onboardCan.SetBitTiming(new CanBitTiming(propagationPhase1: 13, phase2: 2, baudratePrescaler: 6, synchronizationJumpWidth: 1, useMultiBitSampling: false));
            _onboardCan.Enable();
            _onboardCan.MessageReceived += Can_MessageReceived;
            _onboardCan.ErrorReceived += Can_ErrorReceived;

            CanSpiToOnboardCan();
            OnboardCanToCanSpi();
        }

        private static void OnboardCanToCanSpi()
        {
            var Msg1 = new CanMessage()
            {
                ArbitrationId = 0x00,
                Length = 8,
                IsRemoteTransmissionRequest = false,
                IsExtendedId = false
            };

            for (var i = 0; i < 10; i++)
            {
                Msg1.Data = Encoding.UTF8.GetBytes($"Hello{Msg1.ArbitrationId:X3}");
                Debug.WriteLine($"Onboard CAN sending message 'Hello{Msg1.ArbitrationId:X3}', ID 0x{Msg1.ArbitrationId:X3} --->");
                _onboardCan.WriteMessage(Msg1);
                Msg1.ArbitrationId++;

                Thread.Sleep(200);
            }
        }

        private static void CanSpiToOnboardCan()
        {
            var Msg1 = new CanSpiClick.CanMessage()
            {
                ArbitrationId = 0x100,
                Length = 8,
                IsRemoteTransmissionRequest = false,
                IsExtendedId = false
            };

            for (var i = 0; i < 10; i++)
            {
                Msg1.Data = Encoding.UTF8.GetBytes($"MBN__{Msg1.ArbitrationId:X3}");
                Debug.WriteLine($"CanSpi Click sending message 'MBN__{Msg1.ArbitrationId:X3}', ID 0x{Msg1.ArbitrationId:X3} --->");
                _canSpi.WriteMessage(ref Msg1);
                Msg1.ArbitrationId++;

                Thread.Sleep(200);
            }
        }

        private static void CAN1_MessageReceived(Object sender, CanSpiClick.MessageReceivedEventArgs e)
        {
            var str = Encoding.UTF8.GetString(e.Message.Data);

            Debug.WriteLine("---> CanSpiClick receiving data");
            Debug.WriteLine($"\tArbitration ID : 0x{e.Message.ArbitrationId:X3}");
            Debug.WriteLine($"\tTimestamp : {e.Message.Timestamp.Ticks} ticks");
            Debug.WriteLine($"\tData : {str}");
            Debug.WriteLine("************");
        }

        private static void Can_MessageReceived(CanController sender, MessageReceivedEventArgs e)
        {
            sender.ReadMessage(out CanMessage message);

            var str = Encoding.UTF8.GetString(message.Data);

            Debug.WriteLine("---> Onbard CAN receiving data");
            Debug.WriteLine($"\tArbitration ID : 0x{message.ArbitrationId:X3}");
            Debug.WriteLine($"\tTimestamp : {e.Timestamp.Ticks} ticks");
            Debug.WriteLine($"\tData : {str}");
            Debug.WriteLine("************");
        }

        private static void Can_ErrorReceived(CanController sender, ErrorReceivedEventArgs e) => Debug.WriteLine("Onboard Can error : " + e.ToString());
    }
}

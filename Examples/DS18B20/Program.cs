using System;
using MBN;
using MBN.Modules;

using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace Examples
{
    internal class Program
    {
        private static DS18B20 _ds18B20;

         private static readonly Byte[] Device0 = new Byte[] { 40, 27, 213, 247, 4, 0, 0, 88 };
         private static readonly Byte[] Device1 = new Byte[] { 40, 209, 177, 246, 4, 0, 0, 137 };
         private static readonly Byte[] Device2 = new Byte[] { 40, 206, 219, 247, 4, 0, 0, 213 };

             public static void Main()
             {
                 _ds18B20 = new DS18B20(Hardware.SC20100_2.AnPin);

            // Run this first if you want to obtain a list of current DS18B20 Sensors on the OneWire Bus.
            // To get your 64-Bit DeviceIds Uncomment the line DisplayDeviceIds()

            // I am using three sensors for this Demo1, you will need to change the values of Device0, Device1 and Device2 to reflect your
            // sensor id for the Demo1 Code to work. Or just Comment out "RunDemo1(); and Uncomment RunDemo2();

            //DisplayDeviceIds();

            //RunDemo1();

            RunDemo2();

            new Thread(Capture).Start();

            Thread.Sleep(Timeout.Infinite);
             }

             private static void DisplayDeviceIds()
             {
                 Debug.WriteLine("Displaying DeviceIds");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine(GetDeviceId(device));
                 }
             }

             private static void RunDemo1()
             {
                 Debug.WriteLine("*********DS18B20 Demo**********\n");
                 Debug.WriteLine("Number of Devices: " + _ds18B20.NumberOfDevices() + "\n");

                 Debug.WriteLine("Testing Setting Resolutions");
                 _ds18B20.SetResolution(Device0, DS18B20.Resolution.Resolution9Bit);
                 _ds18B20.SetResolution(Device1, DS18B20.Resolution.Resolution11Bit);
                 _ds18B20.SetResolution(Device2, DS18B20.Resolution.Resolution12Bit);

                 /* Alternate Method 1
                 foreach (byte[] device in _ds18B20.DeviceList)
                 {
                     _ds18B20.SetResolution(device, DS18B20.Resolution.Resolution9Bit);
                 }
                 */

                 /* Alternate Method 2
                 _ds18B20.SetResolutionForAllDevices(DS18B20.Resolution.Resolution9Bit);
                 */

                 Debug.WriteLine("\nSetting Resolutions done, now reading back resolutions.");
                 Debug.WriteLine("Current Resolution for Device 0: " + _ds18B20.GetResolutionString(_ds18B20.GetResolution(Device0)));
                 Debug.WriteLine("Current Resolution for Device 1: " + _ds18B20.GetResolutionString(_ds18B20.GetResolution(Device1)));
                 Debug.WriteLine("Current Resolution for Device 2: " + _ds18B20.GetResolutionString(_ds18B20.GetResolution(Device2)) + "\n");

                 /* Alternate method
                 foreach (byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Current Resolution for " + GetDeviceId(device) + " - "  + _ds18B20.GetResolutionString(_ds18B20.GetResolution(device)));
                 }
                 */

                 Debug.WriteLine("Test - Checking for Parasitic Devices");
                 Debug.WriteLine("Is Parasitic? -" + _ds18B20.IsParasitic(Device0));
                 Debug.WriteLine("Is Parasitic? -" + _ds18B20.IsParasitic(Device1));
                 Debug.WriteLine("Is Parasitic? -" + _ds18B20.IsParasitic(Device2) + "\n");

                 /* Alternate method
                 foreach (byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Is Parasitic? -" + _ds18B20.IsParasitic(device));
                 }
                 */

                 Debug.WriteLine("Test - Resetting Alarms");
                 Debug.WriteLine("Resetting Alarms 0 yields " + _ds18B20.ResetAlarmSettings(Device0));
                 Debug.WriteLine("Resetting Alarms 1 yields " + _ds18B20.ResetAlarmSettings(Device1));
                 Debug.WriteLine("Resetting Alarms 2 yields " + _ds18B20.ResetAlarmSettings(Device2) + "\n");

                 /* ResetAlarmSettings - Alternate Method
                 foreach (byte[] device in _ds18B20.DeviceList)
                 {
                     _ds18B20.ResetAlarmSettings(device);
                 }
                 */

                 Debug.WriteLine("Test - Setting individual Alarms");
                 _ds18B20.SetLowTempertureAlarm(Device0, -55);
                 _ds18B20.SetHighTempertureAlarm(Device0, 100);
                 _ds18B20.SetLowTempertureAlarm(Device1, 124);
                 _ds18B20.SetHighTempertureAlarm(Device1, 125);
                 _ds18B20.SetLowTempertureAlarm(Device2, -55);
                 _ds18B20.SetHighTempertureAlarm(Device2, 125);

                 /* Setting Alarms - alternate method 1
                 _ds18B20.SetBothTemperatureAlarms(Device0, -55, 125);
                 _ds18B20.SetBothTemperatureAlarms(Device0, 30, 125);
                 _ds18B20.SetBothTemperatureAlarms(Device2, -55, 125);
                 */

                 /* Setting Alarms - Alternate method 2
                 _ds18B20.SetAlarmsForAlllDevices(-55, 125);
                 */

                 /* Setting alarms - Alternate method 3
                 foreach (byte[] device in _ds18B20.DeviceList)
                 {
                    _ds18B20.SetBothTemperatureAlarms(device, -55, 125);
                 }
                 */

                 Debug.WriteLine("\nSetting Alarms done, now reading back alarm settings.");
                 Debug.WriteLine("Low Temp Alarm for Device0 - " + _ds18B20.ReadLowTempAlarmSetting(Device0) + "°C");
                 Debug.WriteLine("High Temp Alarm for Device0 - " + _ds18B20.ReadHighTempAlarmSetting(Device0) + "°C");
                 Debug.WriteLine("Low Temp Alarm for Device1 - " + _ds18B20.ReadLowTempAlarmSetting(Device1) + "°C");
                 Debug.WriteLine("High Temp Alarm for Device1 - " + _ds18B20.ReadHighTempAlarmSetting(Device1) + "°C");
                 Debug.WriteLine("Low Temp Alarm for Device2 - " + _ds18B20.ReadLowTempAlarmSetting(Device2) + "°C");
                 Debug.WriteLine("High Temp Alarm for Device2 - " + _ds18B20.ReadHighTempAlarmSetting(Device2) + "°C\n");

                 /* Read Alarm Settings - Alternate method
                 foreach (byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Low Temp Alarm for Device - " + GetDeviceId(device) + " - " + _ds18B20.ReadLowTempAlarmSetting(device));
                     Debug.WriteLine("High Temp Alarm for Device - " + GetDeviceId(device) + " - " + _ds18B20.ReadHighTempAlarmSetting(device));
                 }
                 */

                 Debug.WriteLine("Only Device 1 (" + GetDeviceId(Device1) + ") will have an alarm when reading the temperature.\n");
             }

             private static void RunDemo2()
             {
                 Debug.WriteLine("*********DS18B20 Demo**********\n");
                 Debug.WriteLine("Number of Devices: " + _ds18B20.NumberOfDevices() + "\n");

                 Debug.WriteLine("Testing Setting Resolutions");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     _ds18B20.SetResolution(device, DS18B20.Resolution.Resolution9Bit);
                 }

                 Debug.WriteLine("Setting Resolutions done, now reading back resolutions.");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Current Resolution for " + GetDeviceId(device) + " - " + _ds18B20.GetResolutionString(_ds18B20.GetResolution(device)));
                 }

                 Debug.WriteLine("\nTest - Checking for Parasitic Devices");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Is Parasitic? -" + GetDeviceId(device) + " - " + _ds18B20.IsParasitic(device));
                 }

                 Debug.WriteLine("\nTest - Resetting Alarms");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Resetting Alarms for Device " + GetDeviceId(device) + " yields " + _ds18B20.ResetAlarmSettings(device));
                 }

                 Debug.WriteLine("\nTest - Setting all Alarms at once");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     _ds18B20.SetBothTemperatureAlarms(device, 100, 125);
                 }

                 Debug.WriteLine("\nSetting Alarms done, now reading back alarm settings.");
                 foreach (Byte[] device in _ds18B20.DeviceList)
                 {
                     Debug.WriteLine("Low Temp Alarm for Device - " + GetDeviceId(device) + " - " + _ds18B20.ReadLowTempAlarmSetting(device));
                     Debug.WriteLine("High Temp Alarm for Device - " + GetDeviceId(device) + " - " + _ds18B20.ReadHighTempAlarmSetting(device));
                 }

                 Debug.WriteLine("\nAll Devices will have an alarm when reading the temperature.\n");

             }

             private static void Capture()
             {
                 while (true)
                 {
                     Debug.WriteLine("Reading temperatures by DeviceID");

                     foreach (Byte[] id in _ds18B20.DeviceList)
                     {
                         Debug.WriteLine("By ID - Device Address - " + GetDeviceId(id) + ", Temperature - " + _ds18B20.ReadTemperatureByAddress(id) + "°C");
                     }

                     Debug.WriteLine("\nReading all temperatures using ReadTemperatureForAllDevices() method");

                     Hashtable temperature = _ds18B20.ReadTemperatureForAllDevices();

                     foreach (DictionaryEntry t in temperature)
                     {
                         Debug.WriteLine("Key - " + GetDeviceId((Byte[])t.Key) + " Value - " + t.Value);
                     }

                     Debug.WriteLine("\nReadTemperature (single device) - " + _ds18B20.ReadTemperature() + "°C");

                     Debug.WriteLine("\nChecking for devices in alarm - Method 1");

                     foreach (Byte[] alarmingDevice in _ds18B20.AlarmList())
                     {
                         Debug.WriteLine("Alarming Device Address - " + GetDeviceId(alarmingDevice));
                     }

                     Debug.WriteLine("\nChecking for devices in alarm - Method 2");

                     foreach (Byte[] alarmingDevice in _ds18B20.AlarmList())
                     {
                         Debug.WriteLine("Devices in alarm:  " + _ds18B20.HasAlarm(alarmingDevice) + " - " + GetDeviceId(alarmingDevice));
                     }

                     Debug.WriteLine("\n***************************************************************************************************");
                     Thread.Sleep(5000);
                 }
             }

             private static String GetDeviceId(Byte[] id)
             {
                 return id[0] + "," + id[1] + "," + id[2] + "," + id[3] + "," + id[4] + "," + id[5] + "," + id[6] + "," + id[7];
             }
    }
}

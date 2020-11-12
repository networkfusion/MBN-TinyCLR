/* 
* NMEA parser for TinyCLR 2.0
* 
* Version 1.0 : 
* - Initial revision
* 
* Copyright 2020 MBNSoftware.Net 
* Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at 
* http://www.apache.org/licenses/LICENSE-2.0 
* Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
* either express or implied. See the License for the specific language governing permissions and limitations under the License. 
*/

using System;
using System.Collections;
using System.Diagnostics;

namespace TinyCLRRegexp
{
    public static class NMEAParser
    {
        #region Public structs
        public struct RMC
        {
            public Byte TalkerID;
            public DateTime FixTime;
            public Char Status;
            public Single Latitude;
            public Char LatitudeHemisphere;
            public Single Longitude;
            public Char LongitudePosition;
            public Single SpeedKnots;
            public Single SpeedKm;
            public Single TrackAngle;
            public Single MagneticVariation;
            public Char MagneticVariationDirection;
            public Byte Checksum;
        }

        public struct GGA
        {
            public Byte TalkerID;
            public DateTime FixTime;
            public Single Latitude;
            public Char LatitudeHemisphere;
            public Single Longitude;
            public Char LongitudePosition;
            public Byte QualityIndicator;
            public Byte SatellitesInUse;
            public Single HorizontalDilution;
            public Single AntennaAltitude;
            public Char AntennaAltitudeUnit;
            public Single GeoidalSeparation;
            public Char GeoidalSeparationUnit;
            public Single AgeOfDifferentialData;
            public Int32 DifferentialReferenceStationID;
            public Byte Checksum;
        }

        public struct GSA
        {
            public Byte TalkerID;
            public Char SelectionMode;
            public Byte Mode;
            public Int32 Satellite1Id;
            public Int32 Satellite2Id;
            public Int32 Satellite3Id;
            public Int32 Satellite4Id;
            public Int32 Satellite5Id;
            public Int32 Satellite6Id;
            public Int32 Satellite7Id;
            public Int32 Satellite8Id;
            public Int32 Satellite9Id;
            public Int32 Satellite10Id;
            public Int32 Satellite11Id;
            public Int32 Satellite12Id;
            public Single PDOP;
            public Single HDOP;
            public Single VDOP;
            public Byte Checksum;
        }

        public struct GSV
        {
            public Byte TalkerID;
            public Byte NumberOfSentences;
            public Byte SequenceNumber;
            public Byte SatellitesInView;
            public Int32 Satellite1Id;
            public Int32 Satellite1Elevation;
            public Int32 Satellite1Azimuth;
            public Byte Satellite1SNR;
            public Int32 Satellite2Id;
            public Int32 Satellite2Elevation;
            public Int32 Satellite2Azimuth;
            public Byte Satellite2SNR;
            public Int32 Satellite3Id;
            public Int32 Satellite3Elevation;
            public Int32 Satellite3Azimuth;
            public Byte Satellite3SNR;
            public Int32 Satellite4Id;
            public Int32 Satellite4Elevation;
            public Int32 Satellite4Azimuth;
            public Byte Satellite4SNR;
            public Byte Checksum;
        }

        public struct VTG
        {
            public Byte TalkerID;
            public Single CourseOverGroundDegrees;
            public Single CourseOverGroundMagnetic;
            public Single SpeedOverGroundKnots;
            public Single SpeedOverGroundKm;
            public Byte Checksum;
        }

        public struct HDT
        {
            public Byte TalkerID;
            public Single Heading;
            public Byte Checksum;
        }

        #endregion

        #region Public vars
        public static RMC RMCSentence;
        public static GGA GGASentence; 
        public static GSA GSASentence;
        public static GSV[] GSVSentence;
        public static VTG VTGSentence;
        public static HDT HDTSentence;
        #endregion

        #region Constructor
        static NMEAParser()
        {
            tabtmp = new ArrayList();
            arrayResult = new ArrayList();
            RMCSentence = new RMC();
            GGASentence = new GGA();
            GSASentence = new GSA();
            GSVSentence = new GSV[3];
            VTGSentence = new VTG();
            HDTSentence = new HDT();
            lockGGA = new Object();
            lockGSA = new Object();
            lockRMC = new Object();
            lockGSV = new Object();
            lockVTG = new Object();
            lockHDT = new Object();
        }
        #endregion

        #region Private vars
        private static readonly Byte[] patternRMC = new Byte[3] { 82, 77, 67 };
        private static readonly Byte[] patternGGA = new Byte[3] { 71, 71, 65 };
        private static readonly Byte[] patternGSA = new Byte[3] { 71, 83, 65 };
        private static readonly Byte[] patternGSV = new Byte[3] { 71, 83, 86 };
        private static readonly Byte[] patternVTG = new Byte[3] { 86, 84, 71 };
        private static readonly Byte[] patternHDT = new Byte[3] { 72, 68, 84 };

        private static readonly Byte[][] SupportedPatterns = new Byte[][] { patternGGA, patternGSA, patternRMC, patternGSV, patternVTG, patternHDT };
        private static ArrayList NMEAFields = new ArrayList();
        private static readonly Object lockGGA, lockGSA, lockRMC, lockGSV, lockVTG, lockHDT;

        private static readonly ArrayList tabtmp;
        private static readonly ArrayList arrayResult;
        private static Byte b0, b1;
        #endregion

        #region Private methods
        private static ArrayList Split(Byte[] tab, Byte sep)
        {
            for (var i = 0; i < tab.Length; i++)
            {
                if ((tab[i] == sep) || (tab[i] == 42))
                {
                    arrayResult.Add(tabtmp.ToArray(typeof(Byte)));
                    tabtmp.Clear();
                }
                else
                {
                    tabtmp.Add(tab[i]);
                }
            }
            arrayResult.Add(tabtmp.ToArray(typeof(Byte)));

            return arrayResult;
        }


        private static Int32 IndexOf(Byte[] input, Byte[] pattern)
        {
            var firstByte = pattern[0];
            Int32 index;

            if ((index = Array.IndexOf(input, firstByte, 3)) >= 0)
            {
                for (var i = 0; i < pattern.Length; i++)
                {
                    if (index + i >= input.Length || pattern[i] != input[index + i])
                        return -1;
                }
            }

            return index;
        }

        private static Int32 IntFromAscii(Byte[] bytes) => IntFromAscii(bytes, 0, (Byte)bytes.Length);

        private static Int32 IntFromAscii(Byte[] bytes, Byte startIndex, Byte count)
        {
            if (bytes.Length == 0)
                return 0;
            var result = 0;

            for (Byte i = 0; i < count; ++i)
            {
                result += (Int32)((bytes[i + startIndex] - 48) * Math.Pow(10, count - i - 1));
            }

            return result;
        }

        private static Single SingleFromAscii(Byte[] bytes)
        {
            if (bytes.Length == 0)
                return 0.0f;
            var decimalPointPosition = Array.IndexOf(bytes, 46);
            Array.Copy(bytes, decimalPointPosition + 1, bytes, decimalPointPosition, bytes.Length - decimalPointPosition - 1);
            bytes[bytes.Length - 1] = 48;

            return (Single)(IntFromAscii(bytes, 0, (Byte)bytes.Length) / Math.Pow(10, bytes.Length - decimalPointPosition));
        }

        #endregion

        #region private NMEA parsing methods
        private static void ParseRMC(Byte[] sentence)
        {
            NMEAFields.Clear();
            NMEAFields = Split(sentence, 44);
            lock (lockRMC)
            {
                RMCSentence.FixTime = new DateTime(
                    IntFromAscii((Byte[])NMEAFields[9], 4, 2)+2000,
                    IntFromAscii((Byte[])NMEAFields[9], 2, 2),
                    IntFromAscii((Byte[])NMEAFields[9], 0, 2),
                    IntFromAscii((Byte[])NMEAFields[1], 0, 2),
                    IntFromAscii((Byte[])NMEAFields[1], 2, 2),
                    IntFromAscii((Byte[])NMEAFields[1], 4, 2));
                RMCSentence.Status = (Char)((Byte[])NMEAFields[2])[0];
                RMCSentence.Latitude = SingleFromAscii((Byte[])NMEAFields[3]) / 100.0f;
                RMCSentence.LatitudeHemisphere = (Char)((Byte[])NMEAFields[4])[0];
                RMCSentence.Longitude = SingleFromAscii((Byte[])NMEAFields[5]) / 100.0f;
                RMCSentence.LongitudePosition = (Char)((Byte[])NMEAFields[6])[0];
                RMCSentence.SpeedKnots = SingleFromAscii((Byte[])NMEAFields[7]);
                RMCSentence.SpeedKm = RMCSentence.SpeedKnots * 1.852f;
                RMCSentence.TrackAngle = SingleFromAscii((Byte[])NMEAFields[8]);
                RMCSentence.MagneticVariation = SingleFromAscii((Byte[])NMEAFields[10]);
                RMCSentence.MagneticVariationDirection = (Char)((Byte[])NMEAFields[11])[0];

                b0 = (Byte)(((Byte[])NMEAFields[12])[0] >= 65 ? ((Byte[])NMEAFields[12])[0] - 55 : ((Byte[])NMEAFields[12])[0] - 48);
                b1 = (Byte)(((Byte[])NMEAFields[12])[1] >= 65 ? ((Byte[])NMEAFields[12])[1] - 55 : ((Byte[])NMEAFields[12])[1] - 48);
                RMCSentence.Checksum = (Byte)((b0 << 4) + b1);
            }
        }

        private static void ParseGGA(Byte[] sentence)
        {
            NMEAFields.Clear();
            NMEAFields = Split(sentence, 44);
            lock (lockGGA)
            {
                GGASentence.FixTime = new DateTime(
                    DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                    IntFromAscii((Byte[])NMEAFields[1], 0, 2),
                    IntFromAscii((Byte[])NMEAFields[1], 2, 2),
                    IntFromAscii((Byte[])NMEAFields[1], 4, 2));
                GGASentence.Latitude = SingleFromAscii((Byte[])NMEAFields[2]) / 100.0f;
                GGASentence.LatitudeHemisphere = (Char)((Byte[])NMEAFields[3])[0];
                GGASentence.Longitude = SingleFromAscii((Byte[])NMEAFields[4]) / 100.0f;
                GGASentence.LongitudePosition = (Char)((Byte[])NMEAFields[5])[0];
                GGASentence.QualityIndicator = (Byte)(((Byte[])NMEAFields[6])[0]- 48);
                GGASentence.SatellitesInUse = (Byte)(IntFromAscii((Byte[])NMEAFields[7],0,2));
                GGASentence.HorizontalDilution = SingleFromAscii((Byte[])NMEAFields[8]);
                GGASentence.AntennaAltitude = SingleFromAscii((Byte[])NMEAFields[9]);
                GGASentence.AntennaAltitudeUnit = (Char)((Byte[])NMEAFields[10])[0];
                GGASentence.GeoidalSeparation = SingleFromAscii((Byte[])NMEAFields[11]);
                GGASentence.GeoidalSeparationUnit = (Char)((Byte[])NMEAFields[12])[0];
                GGASentence.AgeOfDifferentialData = SingleFromAscii((Byte[])NMEAFields[13]);
                GGASentence.DifferentialReferenceStationID = IntFromAscii((Byte[])NMEAFields[14], 0, 4);

                b0 = (Byte)(((Byte[])NMEAFields[15])[0] >= 65 ? ((Byte[])NMEAFields[15])[0] - 55 : ((Byte[])NMEAFields[15])[0] - 48);
                b1 = (Byte)(((Byte[])NMEAFields[15])[1] >= 65 ? ((Byte[])NMEAFields[15])[1] - 55 : ((Byte[])NMEAFields[15])[1] - 48);
                GGASentence.Checksum = (Byte)((b0 << 4) + b1);
            }
        }

        private static void ParseGSA(Byte[] sentence)
        {
            NMEAFields.Clear();
            NMEAFields = Split(sentence, 44);
            lock (lockGSA)
            {
                GSASentence.SelectionMode = (Char)((Byte[])NMEAFields[1])[0];
                GSASentence.Mode = (Byte)(((Byte[])NMEAFields[2])[0] - 48);
                GSASentence.Satellite1Id = IntFromAscii((Byte[])NMEAFields[3]);
                GSASentence.Satellite2Id = IntFromAscii((Byte[])NMEAFields[4]);
                GSASentence.Satellite3Id = IntFromAscii((Byte[])NMEAFields[5]);
                GSASentence.Satellite4Id = IntFromAscii((Byte[])NMEAFields[6]);
                GSASentence.Satellite5Id = IntFromAscii((Byte[])NMEAFields[7]);
                GSASentence.Satellite6Id = IntFromAscii((Byte[])NMEAFields[8]);
                GSASentence.Satellite7Id = IntFromAscii((Byte[])NMEAFields[9]);
                GSASentence.Satellite8Id = IntFromAscii((Byte[])NMEAFields[10]);
                GSASentence.Satellite9Id = IntFromAscii((Byte[])NMEAFields[11]);
                GSASentence.Satellite10Id = IntFromAscii((Byte[])NMEAFields[12]);
                GSASentence.Satellite11Id = IntFromAscii((Byte[])NMEAFields[13]);
                GSASentence.Satellite12Id = IntFromAscii((Byte[])NMEAFields[14]);
                GSASentence.PDOP = SingleFromAscii((Byte[])NMEAFields[15]);
                GSASentence.HDOP = SingleFromAscii((Byte[])NMEAFields[16]);
                GSASentence.VDOP = SingleFromAscii((Byte[])NMEAFields[17]);

                b0 = (Byte)(((Byte[])NMEAFields[18])[0] >= 65 ? ((Byte[])NMEAFields[18])[0] - 55 : ((Byte[])NMEAFields[18])[0] - 48);
                b1 = (Byte)(((Byte[])NMEAFields[18])[1] >= 65 ? ((Byte[])NMEAFields[18])[1] - 55 : ((Byte[])NMEAFields[18])[1] - 48);
                GSASentence.Checksum = (Byte)((b0 << 4) + b1);
            }
        }

        private static void ParseGSV(Byte[] sentence)
        {
            NMEAFields.Clear();
            NMEAFields = Split(sentence, 44);
            var Seq = ((Byte[])NMEAFields[2])[0] - 49;
            lock (lockGSV)
            {
                GSVSentence[Seq].NumberOfSentences = (Byte)(((Byte[])NMEAFields[1])[0] - 48);
                GSVSentence[Seq].SequenceNumber = (Byte)(Seq + 1);
                GSVSentence[Seq].SatellitesInView = (Byte)IntFromAscii((Byte[])NMEAFields[3]);

                GSVSentence[Seq].Satellite1Id = IntFromAscii((Byte[])NMEAFields[4]);
                GSVSentence[Seq].Satellite1Elevation = IntFromAscii((Byte[])NMEAFields[5]);
                GSVSentence[Seq].Satellite1Azimuth = IntFromAscii((Byte[])NMEAFields[6]);
                GSVSentence[Seq].Satellite1SNR = (Byte)IntFromAscii((Byte[])NMEAFields[7]);

                GSVSentence[Seq].Satellite2Id = IntFromAscii((Byte[])NMEAFields[8]);
                GSVSentence[Seq].Satellite2Elevation = IntFromAscii((Byte[])NMEAFields[9]);
                GSVSentence[Seq].Satellite2Azimuth = IntFromAscii((Byte[])NMEAFields[10]);
                GSVSentence[Seq].Satellite2SNR = (Byte)IntFromAscii((Byte[])NMEAFields[11]);

                GSVSentence[Seq].Satellite3Id = IntFromAscii((Byte[])NMEAFields[12]);
                GSVSentence[Seq].Satellite3Elevation = IntFromAscii((Byte[])NMEAFields[13]);
                GSVSentence[Seq].Satellite3Azimuth = IntFromAscii((Byte[])NMEAFields[14]);
                GSVSentence[Seq].Satellite3SNR = (Byte)IntFromAscii((Byte[])NMEAFields[15]);

                GSVSentence[Seq].Satellite4Id = IntFromAscii((Byte[])NMEAFields[16]);
                GSVSentence[Seq].Satellite4Elevation = IntFromAscii((Byte[])NMEAFields[17]);
                GSVSentence[Seq].Satellite4Azimuth = IntFromAscii((Byte[])NMEAFields[18]);
                GSVSentence[Seq].Satellite4SNR = (Byte)IntFromAscii((Byte[])NMEAFields[19]);


                b0 = (Byte)(((Byte[])NMEAFields[20])[0] >= 65 ? ((Byte[])NMEAFields[20])[0] - 55 : ((Byte[])NMEAFields[20])[0] - 48);
                b1 = (Byte)(((Byte[])NMEAFields[20])[1] >= 65 ? ((Byte[])NMEAFields[20])[1] - 55 : ((Byte[])NMEAFields[20])[1] - 48);
                GSVSentence[Seq].Checksum = (Byte)((b0 << 4) + b1);
            }
        }

        private static void ParseVTG(Byte[] sentence)
        {
            NMEAFields.Clear();
            NMEAFields = Split(sentence, 44);
            lock (lockVTG)
            {
                VTGSentence.CourseOverGroundDegrees = SingleFromAscii((Byte[])NMEAFields[1]);
                VTGSentence.CourseOverGroundMagnetic = SingleFromAscii((Byte[])NMEAFields[3]);
                VTGSentence.SpeedOverGroundKnots = SingleFromAscii((Byte[])NMEAFields[5]);
                VTGSentence.SpeedOverGroundKm = SingleFromAscii((Byte[])NMEAFields[7]);

                b0 = (Byte)(((Byte[])NMEAFields[10])[0] >= 65 ? ((Byte[])NMEAFields[10])[0] - 55 : ((Byte[])NMEAFields[10])[0] - 48);
                b1 = (Byte)(((Byte[])NMEAFields[10])[1] >= 65 ? ((Byte[])NMEAFields[10])[1] - 55 : ((Byte[])NMEAFields[10])[1] - 48);
                VTGSentence.Checksum = (Byte)((b0 << 4) + b1);
            }
        }

        private static void ParseHDT(Byte[] sentence)
        {
            NMEAFields.Clear();
            NMEAFields = Split(sentence, 44);
            lock (lockHDT)
            {
                HDTSentence.Heading = SingleFromAscii((Byte[])NMEAFields[1]);

                b0 = (Byte)(((Byte[])NMEAFields[3])[0] >= 65 ? ((Byte[])NMEAFields[3])[0] - 55 : ((Byte[])NMEAFields[3])[0] - 48);
                b1 = (Byte)(((Byte[])NMEAFields[3])[1] >= 65 ? ((Byte[])NMEAFields[3])[1] - 55 : ((Byte[])NMEAFields[3])[1] - 48);
                HDTSentence.Checksum = (Byte)((b0 << 4) + b1);
            }
        }
        #endregion

        #region Public methods
        public static void Parse(Byte[] NMEASentence)
        {
            // Determine which NMEA sentence has been received
            var index = -1;
            for (var i = 0; i < SupportedPatterns.Length; i++)
            {
                // A supported frame pattern has been found, get its index and exit the loop
                if (IndexOf(NMEASentence, SupportedPatterns[i]) != -1)
                {
                    index = i;
                    break;
                }
            }
            switch (index)
            {
                case 0:     // GGA sentence
                    ParseGGA(NMEASentence);
                    break;
                case 1:     // GSA sentence
                    ParseGSA(NMEASentence);
                    break;
                case 2:     // RMC sentence
                    ParseRMC(NMEASentence);
                    break;
                case 3:     // GSV sentence
                    ParseGSV(NMEASentence);
                    break;
                case 4:     // VTG sentence
                    ParseVTG(NMEASentence);
                    break;
                case 5:     // HDT sentence
                    ParseHDT(NMEASentence);
                    break;
                default:
                    Debug.WriteLine("Not supported frame received");
                    break;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes
{
    public class PortMeterCommands
    {
        public static readonly string GetMeasurements = "e";
        public static readonly string DeleteMeasurements = "f";
        public static readonly string SetDateTime = "g";
        public static readonly string DefaultMessage = "i";
        public static readonly string FactorySettings = "u";
        public static readonly string GetSerialNumber = "h";
        public static readonly string SaveSerialNumber = "n";
        public static readonly string Calibration = "k";
        public static readonly string EndConnection = "0";

        public static readonly char ReceviedDefaultMessage = 'a';
    }
}

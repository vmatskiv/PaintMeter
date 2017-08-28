using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PaintMeter.Classes.PDFGenerator
{
    public class TemplatePDF
    {
        public static readonly float LeftMargin = 36;
        public static readonly float RightMargin = 36;
        public static readonly float TopMargin = 16;
        public static readonly float BottomMargin = 36;
        public static readonly float ScaledLogoPercent = 30;

        public static readonly int NumberOfColumnsContactDetailsTable = 2;

        public static readonly string LogoPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Resources\\logo.png";

        public static readonly Font titleFontContactDetailsTable = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, 13, Font.BOLD);
        public static readonly Font cellTitleFontContactDetailsTable = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, 10, Font.BOLD);
        public static readonly Font cellFontValueTable = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, 10, Font.NORMAL);
        public static readonly Font graphicalPresentation = FontFactory.GetFont(BaseFont.ZAPFDINGBATS, BaseFont.CP1257, 10, Font.BOLD);
        public static readonly Font specialSymbolFont = FontFactory.GetFont(BaseFont.SYMBOL, BaseFont.CP1257, 9, Font.NORMAL);

        public static readonly string chartToGraphicalPresentation = "n";

        public enum StuffMaterials
        {
            brakMaterialu, stal, ocynk, aluminium
        }

        public static string ReturnValue(int value)
        {
            string valueString = "";
            if (value == -1)
            {
                valueString = "Nie zapisano";
            }
            else if (value > 0 && value < 10000)
            {
                valueString = value.ToString();
            }
            else
            {
                valueString = "--------";
            }
            return valueString;
        }

        public static string ReturnStuff(int stuff)
        {
            if (stuff == (int)StuffMaterials.aluminium)
                return "Aluminium";
            else if (stuff == (int)StuffMaterials.stal)
                return "Stal";
            else if (stuff == (int)StuffMaterials.ocynk)
                return "Ocynk";
            else if (stuff == (int)StuffMaterials.brakMaterialu)
                return "Brak materiału";
            else
                return "Brak materiału";
        }

        public static string ReturnGraphicalPresentation(string stringValue)
        {
            string stringGraphicalPresnetation = "";
            if (stringValue != "Nie zapisano" && stringValue != "--------")
            {
                int value = Convert.ToInt16(stringValue);
                int cnt = 0;
                if (value > 2000)
                    cnt = 21;
                else
                    cnt = (value / 100);
                for (int i = 0; i < cnt + 1; i++)
                {
                    stringGraphicalPresnetation += chartToGraphicalPresentation;
                }
            }
            return stringGraphicalPresnetation;
        }

        public static string ReturnDateTimeMeasurement(string dateTime)
        {
            string newDateTime = "";
            string[] separators = { " " };
            string[] words = dateTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            newDateTime = words[0] + " / " + words[1];
            return newDateTime;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes
{
    public class MessageToUser
    {
        public static readonly string saveException = "Nie udało się zapisać danych wynikowych.";
        public static readonly string openException = "Nie udało się odtworzyć danych historycznych";
        public static readonly string writeToPortException = "Próba komunikacji z urządzeniem nie powidoła się";
    }
}

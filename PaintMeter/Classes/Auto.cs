using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes
{
    public class Auto
    {
        [DisplayName("Numer rejestracyjny")]
        public string RegisterNumber { get; set; }
        [DisplayName("Numer VIN")]
        public string ChassisNumber { get; set; }
        [DisplayName("Model")]
        public string Model { get; set; }
        [DisplayName("Producent")]
        public string Manufacturer { get; set; }
        [DisplayName("Rok produkcji")]
        public string YearOfProduction { get; set; }
        [DisplayName("Przebieg")]
        public string Course { get; set; }
    }
}

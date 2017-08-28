using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes
{
    public class Measurement
    {
        [DisplayName("Lp.")]
        public string Number { get; set; }
        [DisplayName("Wartość:")]
        public string Value { get; set; }
        [DisplayName("Materiał:")]
        public string Stuff { get; set; }
        [DisplayName("Data pomiaru")]
        public string DateTimeMeasurement { get; set; }
    }
}

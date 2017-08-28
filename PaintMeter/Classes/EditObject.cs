using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes
{
    public class EditObject
    {
        public string RegisterNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string YearOfProduction { get; set; }
        public string Course { get; set; }
        public string Name { get; set; }
        public string OtherData { get; set; }
        public string ModelOfProduct { get; set; }
        public string ManufacturerOfProduct { get; set; }
        public string NumberSerial { get; set; }
        public string Conclusions { get; set; }
        public ObservableCollection<Measurement> collectionMeasurements { get; set; }
    }
}

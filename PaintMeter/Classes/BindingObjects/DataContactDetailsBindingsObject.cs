using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes.BindingObjects
{
    public class DataContactDetailsBindingsObject : INotifyPropertyChanged
    {
        #region Name
        public const string NamePropertyName = "Name";
        private string _name = "";
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }
        #endregion

        #region Other
        public const string OtherPropertyName = "Other";
        private string _other = "";
        public string Other
        {
            get
            {
                return _other;
            }

            set
            {
                if (_other == value)
                {
                    return;
                }

                _other = value;
                RaisePropertyChanged(OtherPropertyName);
            }
        }
        #endregion

        #region Model
        public const string ModelPropertyName = "Model";
        private string _model = "";
        public string Model
        {
            get
            {
                return _model;
            }

            set
            {
                if (_model == value)
                {
                    return;
                }

                _model = value;
                RaisePropertyChanged(ModelPropertyName);
            }
        }
        #endregion

        #region Manufacturer
        public const string ManufacturerPropertyName = "Manufacturer";
        private string _manufacturer = "";
        public string Manufacturer
        {
            get
            {
                return _manufacturer;
            }

            set
            {
                if (_manufacturer == value)
                {
                    return;
                }

                _manufacturer = value;
                RaisePropertyChanged(ManufacturerPropertyName);
            }
        }
        #endregion

        #region SerialNumber
        public const string SerialNumberPropertyName = "SerialNumber";
        private string _serialNumber = "";
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }

            set
            {
                if (_serialNumber == value)
                {
                    return;
                }

                _serialNumber = value;
                RaisePropertyChanged(SerialNumberPropertyName);
            }
        }
        #endregion

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

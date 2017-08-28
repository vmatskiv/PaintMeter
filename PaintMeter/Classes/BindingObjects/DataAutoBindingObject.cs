using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes.BindingObjects
{
    public class DataAutoBindingObject : INotifyPropertyChanged
    {
        #region RegisterNumber
        public const string RegisterNumberPropertyName = "RegisterNumber";
        private string _registerNumber = "";
        public string RegisterNumber
        {
            get
            {
                return _registerNumber;
            }

            set
            {
                if (_registerNumber == value)
                {
                    return;
                }

                _registerNumber = value;
                RaisePropertyChanged(RegisterNumberPropertyName);
            }
        }
        #endregion

        #region ChassisNumber
        public const string ChassisNumberPropertyName = "ChassisNumber";
        private string _chassisNumber = "";
        public string ChassisNumber
        {
            get
            {
                return _chassisNumber;
            }

            set
            {
                if (_chassisNumber == value)
                {
                    return;
                }

                _chassisNumber = value;
                RaisePropertyChanged(ChassisNumberPropertyName);
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

        #region YearOfProduction
        public const string YearOfProductionPropertyName = "YearOfProduction";
        private string _yearOfProduction = "";
        public string YearOfProduction
        {
            get
            {
                return _yearOfProduction;
            }

            set
            {
                if (_yearOfProduction == value)
                {
                    return;
                }

                _yearOfProduction = value;
                RaisePropertyChanged(YearOfProductionPropertyName);
            }
        }
        #endregion

        #region Course
        public const string CoursePropertyName = "Course";
        private string _course = "";
        public string Course
        {
            get
            {
                return _course;
            }

            set
            {
                if (_course == value)
                {
                    return;
                }

                _course = value;
                RaisePropertyChanged(CoursePropertyName);
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

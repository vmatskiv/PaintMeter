using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PaintMeter.Classes.BindingObjects
{
    public class ConclusionsBindingObject : INotifyPropertyChanged
    {
        #region Conclusions
        public const string ConclusionsPropertyName = "Conclusions";
        private string _conclusions = "";
        public string Conclusions
        {
            get
            {
                return _conclusions;
            }

            set
            {
                if (_conclusions == value)
                {
                    return;
                }

                _conclusions = value;
                RaisePropertyChanged(ConclusionsPropertyName);
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

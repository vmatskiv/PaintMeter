using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PaintMeter.Classes;
using PaintMeter.Classes.BindingObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace PaintMeter.ViewModel
{
    public class EditionViewModel : ViewModelBase
    {
        private BackgroundWorker saveChangesWorker;
        private string oldRegisterNumber = "";
        private bool manualClose = true;

        #region INotifyPoperties

        #region DataAuto
        public const string DataAutoPropertyName = "DataAuto";
        private DataAutoBindingObject _dataAuto = new DataAutoBindingObject();
        public DataAutoBindingObject DataAuto
        {
            get
            {
                return _dataAuto;
            }
            set
            {
                if (_dataAuto == value)
                {
                    return;
                }
                _dataAuto = value;
                RaisePropertyChanged(DataAutoPropertyName);
            }
        }
        #endregion

        #region DataContactDetails
        public const string DataContactDetailsPropertyName = "DataContactDetails";
        private DataContactDetailsBindingsObject _dataContactDetails = new DataContactDetailsBindingsObject();
        public DataContactDetailsBindingsObject DataContactDetails
        {
            get
            {
                return _dataContactDetails;
            }
            set
            {
                if (_dataContactDetails == value)
                {
                    return;
                }
                _dataContactDetails = value;
                RaisePropertyChanged(DataContactDetailsPropertyName);
            }
        }
        #endregion

        #region ConclusionsBindingModel
        public const string ConclusionsBindingModelPropertyName = "ConclusionsBindingModel";
        private ConclusionsBindingObject _conclusionsBindingModels = new ConclusionsBindingObject();
        public ConclusionsBindingObject ConclusionsBindingModel
        {
            get
            {
                return _conclusionsBindingModels;
            }
            set
            {
                if (_conclusionsBindingModels == value)
                {
                    return;
                }
                _conclusionsBindingModels = value;
                RaisePropertyChanged(ConclusionsBindingModelPropertyName);
            }
        }
        #endregion

        #region ItemSourceHistoricalDataGrid
        public const string ItemSourceHistoricalDataGridPropertyName = "ItemSourceHistoricalDataGrid";
        private ObservableCollection<Measurement> _itemSourceHistoricalDataGrid = new ObservableCollection<Measurement>();
        public ObservableCollection<Measurement> ItemSourceHistoricalDataGrid
        {
            get
            {
                return _itemSourceHistoricalDataGrid;
            }
            set
            {
                if (_itemSourceHistoricalDataGrid == value)
                {
                    return;
                }
                _itemSourceHistoricalDataGrid = value;
                RaisePropertyChanged(ItemSourceHistoricalDataGridPropertyName);
            }
        }
        #endregion

        #endregion

        #region RelayCommands

        #region WindowClosing
        private RelayCommand _windowClosing;
        public RelayCommand WindowClosing
        {
            get
            {
                return _windowClosing
                  ?? (_windowClosing = new RelayCommand(
                    () =>
                    {
                        //if (manualClose==true)
                        //{
                        //    MessageBoxResult result = MessageBox.Show("Zapisać zmiany?", "", MessageBoxButton.YesNo);
                        //    if (result == MessageBoxResult.Yes)
                        //        saveChangesWorker.RunWorkerAsync();
                        //}
                    }));
            }
        }
        #endregion

        #region CloseAndSaveCommand
        private RelayCommand<Window> _closeAndSaveCommand;
        public RelayCommand<Window> CloseAndSaveCommand
        {
            get
            {
                return _closeAndSaveCommand
                  ?? (_closeAndSaveCommand = new RelayCommand<Window>(
                    (window) =>
                    {
                        manualClose = false;
                        saveChangesWorker.RunWorkerAsync();
                        window.Close();
                    }));
            }
        }
        #endregion

        #region CloseAndDontSaveCommand
        private RelayCommand<Window> _closeAndDontSaveCommand;
        public RelayCommand<Window> CloseAndDontSaveCommand
        {
            get
            {
                return _closeAndDontSaveCommand
                  ?? (_closeAndDontSaveCommand = new RelayCommand<Window>(
                    (window) =>
                    {
                        manualClose = false;
                        window.Close();
                    }));
            }
        }
        #endregion

        #endregion

        public EditionViewModel()
        {
            Messenger.Default.Register<NotificationMessage<EditObject>>(this, NotificationMessageReceived);

            saveChangesWorker = new BackgroundWorker();
            saveChangesWorker.DoWork += Save;
        }

        private void NotificationMessageReceived(NotificationMessage<EditObject> msg)
        {
            if (msg.Notification == "SendEditionObject")
            {
                OpenEditObject(msg.Content);
                oldRegisterNumber = DataAuto.RegisterNumber;
            }
        }

        private void OpenEditObject(EditObject editObject)
        {
            DataAuto.RegisterNumber = editObject.RegisterNumber;
            DataAuto.ChassisNumber = editObject.ChassisNumber;
            DataAuto.Model = editObject.Model;
            DataAuto.Manufacturer = editObject.Manufacturer;
            DataAuto.YearOfProduction = editObject.YearOfProduction;
            DataAuto.Course = editObject.Course;

            DataContactDetails.Name = editObject.Name;
            DataContactDetails.Other = editObject.OtherData;
            DataContactDetails.Model = editObject.ModelOfProduct;
            DataContactDetails.Manufacturer = editObject.ManufacturerOfProduct;
            DataContactDetails.SerialNumber = editObject.NumberSerial;

            ConclusionsBindingModel.Conclusions = editObject.Conclusions;

            ItemSourceHistoricalDataGrid = editObject.collectionMeasurements;
        }

        private void Save(object sender, DoWorkEventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            Serializer serializer = new Serializer();
            SaveAuto(serializer);
            SaveDataConcatDetails(serializer);
            SaveConclusions(serializer);
            UpdateMeasurementsFile(serializer);
            Messenger.Default.Send(new NotificationMessage<bool>(true, "RefreshAutosList"));
        }

        private void UpdateMeasurementsFile(Serializer serializer)
        {
            if (oldRegisterNumber != DataAuto.RegisterNumber)
            {
                var measurements = serializer.GetMeasurementsFromFile(oldRegisterNumber);
                serializer.SerializeCollectionMeasurements(measurements, DataAuto.RegisterNumber);
                serializer.DeleteFiles(oldRegisterNumber);
            }
        }

        private void SaveAuto(Serializer serializer)
        {
            Auto auto = new Auto();
            auto.RegisterNumber = DataAuto.RegisterNumber;
            auto.ChassisNumber = DataAuto.ChassisNumber;
            auto.Model = DataAuto.Model;
            auto.Manufacturer = DataAuto.Manufacturer;
            auto.YearOfProduction = DataAuto.YearOfProduction;
            auto.Course = DataAuto.Course;

            serializer.SerializeAuto(auto, auto.RegisterNumber);
        }

        private void SaveDataConcatDetails(Serializer serializer)
        {
            ContactDetails contactDetails = new ContactDetails();
            contactDetails.Name = DataContactDetails.Name;
            contactDetails.OtherData = DataContactDetails.Other;
            contactDetails.ProductModel = DataContactDetails.Model;
            contactDetails.ManufaturerProduct = DataContactDetails.Manufacturer;
            contactDetails.SerialNumberProduct = DataContactDetails.SerialNumber;

            serializer.SerializeDataDetails(contactDetails, DataAuto.RegisterNumber);
        }

        private void SaveConclusions(Serializer serializer)
        {
            serializer.SerializeConclusions(ConclusionsBindingModel.Conclusions, DataAuto.RegisterNumber);
        }
    }
}

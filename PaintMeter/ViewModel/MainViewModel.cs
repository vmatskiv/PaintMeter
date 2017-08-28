using System;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using PaintMeter.Classes;
using PaintMeter.Model;
using PaintMeter.View;
using System.Windows.Media;
using PaintMeter.Classes.BindingObjects;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using PaintMeter.Classes.PDFGenerator;
using System.IO.Ports;
using System.Windows.Threading;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace PaintMeter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private readonly IDataService _dataService;
        // private Communications comunication = new Communications();
        private Comunication comunication = new Comunication();
        private Serializer serializer = new Serializer();
        GeneratorPDF generator = new GeneratorPDF();

        #endregion

        #region NotificationMessageRecieved

        private void NotificationMessageReceived(NotificationMessage<bool> msg)
        {
            if (msg.Notification == "ConnectStatus")
            {
                SetConnectStatus(msg.Content);
            }
            else if (msg.Notification == "RefreshAutosList")
            {
                openHistoricalDataWorker.RunWorkerAsync();
            }
        }
        private void NotificationMessageReceived(NotificationMessage<string> msg)
        {
            if (msg.Notification == "UpdateSerialNumber")
            {
                string[] separators = { ";", " " };
                string[] words = msg.Content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                DataContactDetails.Model = words[0];
                DataContactDetails.Manufacturer = "Blue Technology";
                DataContactDetails.SerialNumber = words[1];
            }
        }

        #endregion

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

        #region IndexGetMeasurements
        public const string IndexGetMeasurementsPropertyName = "IndexGetMeasurements";
        private int _indexGetMeasurements = 0;
        public int IndexGetMeasurements
        {
            get
            {
                return _indexGetMeasurements;
            }
            set
            {
                if (_indexGetMeasurements == value)
                {
                    return;
                }
                _indexGetMeasurements = value;
                RaisePropertyChanged(IndexGetMeasurementsPropertyName);
            }
        }
        #endregion

        #region IndexDeleteMeasurements
        public const string IndexDeleteMeasurementsPropertyName = "IndexDeleteMeasurements";
        private int _indexDeleteMeasurements = 0;
        public int IndexDeleteMeasurements
        {
            get
            {
                return _indexDeleteMeasurements;
            }
            set
            {
                if (_indexDeleteMeasurements == value)
                {
                    return;
                }
                _indexDeleteMeasurements = value;
                RaisePropertyChanged(IndexDeleteMeasurementsPropertyName);
            }
        }
        #endregion

        #region Conclusions
        public const string ConclusionsPropertyName = "Conclusions";
        private string _conclusions = null;
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

        #region ItemSourceHistoricalDataGrid
        public const string ItemSourceHistoricalDataGridPropertyName = "ItemSourceHistoricalDataGrid";
        private ObservableCollection<Auto> _itemSourceHistoricalDataGrid = new ObservableCollection<Auto>();
        public ObservableCollection<Auto> ItemSourceHistoricalDataGrid
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

        #region IndexOfHistroicalDataGrid
        public const string IIndexOfHistroicalDataGridPropertyName = "IndexOfHistroicalDataGrid";
        private int _indexOfHistroicalDataGrid = 0;
        public int IndexOfHistroicalDataGrid
        {
            get
            {
                return _indexOfHistroicalDataGrid;
            }
            set
            {
                if (_indexOfHistroicalDataGrid == value)
                {
                    return;
                }
                _indexOfHistroicalDataGrid = value;
                RaisePropertyChanged(IIndexOfHistroicalDataGridPropertyName);
            }
        }
        #endregion

        #region ItemSourceMeasurementsDataGrid
        public const string ItemSourceMeasurementsDataGridPropertyName = "ItemSourceMeasurementsDataGrid";
        private ObservableCollection<Measurement> _itemSourceMeasurementsDataGrid = new ObservableCollection<Measurement>();
        public ObservableCollection<Measurement> ItemSourceMeasurementsDataGrid
        {
            get
            {
                return _itemSourceMeasurementsDataGrid;
            }
            set
            {
                if (_itemSourceMeasurementsDataGrid == value)
                {
                    return;
                }
                _itemSourceMeasurementsDataGrid = value;
                RaisePropertyChanged(ItemSourceMeasurementsDataGridPropertyName);
            }
        }
        #endregion

        #region SetDateTime
        public const string SetDateTimePropertyName = "SetDateTime";
        private DateTime _setDateTime = DateTime.Now;
        public DateTime SetDateTime
        {
            get
            {
                return _setDateTime;
            }
            set
            {
                if (_setDateTime == value)
                {
                    return;
                }
                _setDateTime = value;
                RaisePropertyChanged(SetDateTimePropertyName);
            }
        }
        #endregion

        #region IndexOfPortsCom
        public const string IndexOfPortsComPropertyName = "IndexOfPortsCom";
        private int _indexOfPortsCom = 0;
        public int IndexOfPortsCom
        {
            get
            {
                return _indexOfPortsCom;
            }
            set
            {
                if (_indexOfPortsCom == value)
                {
                    return;
                }
                _indexOfPortsCom = value;
                RaisePropertyChanged(IndexOfPortsComPropertyName);
            }
        }
        #endregion

        #region PortsComItems
        public const string PortsComItemsPropertyName = "PortsComItems";
        private ObservableCollection<string> _portsComItems = null;
        public ObservableCollection<string> PortsComItems
        {
            get
            {
                return _portsComItems;
            }
            set
            {
                if (_portsComItems == value)
                {
                    return;
                }
                _portsComItems = value;
                RaisePropertyChanged(PortsComItemsPropertyName);
            }
        }
        #endregion

        #region IsCheckedManualPortCom
        public const string IsCheckedManualPortComPropertyName = "IsCheckedManualPortCom";
        private bool _isCheckedManualPortCom = !Properties.Settings.Default.IsAutomaticSearchPort;
        public bool IsCheckedManualPortCom
        {
            get
            {
                return _isCheckedManualPortCom;
            }
            set
            {
                if (_isCheckedManualPortCom == value)
                {
                    return;
                }
                _isCheckedManualPortCom = value;
                RaisePropertyChanged(IsCheckedManualPortComPropertyName);
            }
        }
        #endregion

        #region IsCheckedAutomaticPortCom
        public const string IsCheckedAutomaticPortComPropertyName = "IsCheckedAutomaticPortCom";
        private bool _isCheckedAutomaticPortCom = Properties.Settings.Default.IsAutomaticSearchPort;
        public bool IsCheckedAutomaticPortCom
        {
            get
            {
                return _isCheckedAutomaticPortCom;
            }
            set
            {
                if (_isCheckedAutomaticPortCom == value)
                {
                    return;
                }
                _isCheckedAutomaticPortCom = value;
                RaisePropertyChanged(IsCheckedAutomaticPortComPropertyName);
                Properties.Settings.Default.IsAutomaticSearchPort = IsCheckedAutomaticPortCom;
                Properties.Settings.Default.Save();
                if (IsCheckedAutomaticPortCom)
                {
                    SearchAndConnectTimer.Start();
                    CheckExistingPortTimer.Stop();
                }
                else
                {
                    SearchAndConnectTimer.Stop();
                    CheckExistingPortTimer.Start();
                }
            }
        }
        #endregion

        #region IsCheckedAutomaticDateTime
        public const string IsCheckedAutomaticDateTimePropertyName = "IsCheckedAutomaticDateTime";
        private bool _isCheckedAutomaticDateTime = Properties.Settings.Default.IsAutomaticDateTime;
        public bool IsCheckedAutomaticDateTime
        {
            get
            {
                return _isCheckedAutomaticDateTime;
            }
            set
            {
                if (_isCheckedAutomaticDateTime == value)
                {
                    return;
                }
                _isCheckedAutomaticDateTime = value;
                RaisePropertyChanged(IsCheckedAutomaticDateTimePropertyName);
                Properties.Settings.Default.IsAutomaticDateTime = IsCheckedAutomaticDateTime;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region IsCheckedManualDateTime
        public const string IsCheckedManualDateTimePropertyName = "IsCheckedManualDateTime";
        private bool _isCheckedManualDateTime = !Properties.Settings.Default.IsAutomaticDateTime;
        public bool IsCheckedManualDateTime
        {
            get
            {
                return _isCheckedManualDateTime;
            }
            set
            {
                if (_isCheckedManualDateTime == value)
                {
                    return;
                }
                _isCheckedManualDateTime = value;
                RaisePropertyChanged(IsCheckedManualDateTimePropertyName);
            }
        }
        #endregion

        #region ConnectionStatus
        public const string ConnectionStatusPropertyName = "ConnectionStatus";
        private string _connectionStatus = "Nie połączono";
        public string ConnectionStatus
        {
            get
            {
                return _connectionStatus;
            }
            set
            {
                if (_connectionStatus == value)
                {
                    return;
                }
                _connectionStatus = value;
                RaisePropertyChanged(ConnectionStatusPropertyName);
            }
        }
        #endregion

        #region IsConnectStatus
        public const string IsConnectStatusPropertyName = "IsConnectStatus";
        private bool _isConnectStatus = false;
        public bool IsConnectStatus
        {
            get
            {
                return _isConnectStatus;
            }
            set
            {
                if (_isConnectStatus == value)
                {
                    return;
                }
                _isConnectStatus = value;
                RaisePropertyChanged(IsConnectStatusPropertyName);
            }
        }
        #endregion

        #region ComunicationStatus
        public const string ComunicationStatusPropertyName = "ComunicationStatus";
        private string _comunicationStatus = "";
        public string ComunicationStatus
        {
            get
            {
                return _comunicationStatus;
            }
            set
            {
                if (_comunicationStatus == value)
                {
                    return;
                }
                _comunicationStatus = value;
                RaisePropertyChanged(ComunicationStatusPropertyName);
            }
        }
        #endregion

        #region ConnectButtonContent
        public const string ConnectButtonContentPropertyName = "ConnectButtonContent";
        private string _connectButtonContent = "Połącz";
        public string ConnectButtonContent
        {
            get
            {
                return _connectButtonContent;
            }
            set
            {
                if (_connectButtonContent == value)
                {
                    return;
                }
                _connectButtonContent = value;
                RaisePropertyChanged(ConnectButtonContentPropertyName);
            }
        }
        #endregion

        #region IsBusy
        public const string IsBusyPropertyName = "IsBusy";
        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                RaisePropertyChanged(IsBusyPropertyName);
            }
        }
        #endregion

        #endregion

        #region RelayCommands

        #region ConnectToMeterCommand
        private RelayCommand _connectToMeterCommand;
        public RelayCommand ConnectToMeterCommand
        {
            get
            {
                return _connectToMeterCommand
                    ?? (_connectToMeterCommand = new RelayCommand(
                    () =>
                    {
                        if (!IsConnectStatus)
                            comunication.SetManualPort(PortsComItems[IndexOfPortsCom]);
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region GetMeasurementsCommand
        private RelayCommand _getMeasurementsCommand;
        public RelayCommand GetMeasurementsCommand
        {
            get
            {
                return _getMeasurementsCommand
                    ?? (_getMeasurementsCommand = new RelayCommand(
                    () =>
                    {
                        if (!measurementsWorker.IsBusy)
                            measurementsWorker.RunWorkerAsync();
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region GeneratePDFCommand
        private RelayCommand _generatePDFCommand;
        public RelayCommand GeneratePDFCommand
        {
            get
            {
                return _generatePDFCommand
                    ?? (_generatePDFCommand = new RelayCommand(
                    () =>
                    {

                        //GeneratorPDF obj = new GeneratorPDF();
                        //Auto auto = new Auto()
                        //{
                        //    ChassisNumber = "123",
                        //    Course = "xyz",
                        //    Manufacturer = "abc",
                        //    Model = "abc",
                        //    RegisterNumber = "abc",
                        //    YearOfProduction = "2017"
                        //};

                        //ContactDetails contactDetails = new ContactDetails()
                        //{
                        //    ManufaturerProduct = "abc",
                        //    Name = "abc",
                        //    OtherData = "abc",
                        //    ProductModel = "abc",
                        //    SerialNumberProduct = "abc"
                        //};

                        //Measurement measurement = new Measurement()
                        //{
                        //    DateTimeMeasurement = String.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now),
                        //    Number = "abc",
                        //    Stuff = "abvc",
                        //    Value = "550"
                        //};

                        //ObservableCollection<Measurement> lstMeasurement = new ObservableCollection<Measurement>();
                        //lstMeasurement.Add(measurement);

                        //obj.CreateDocument(auto, contactDetails, lstMeasurement, "Run it");

                        //return;

                        if (DataAuto.RegisterNumber != null && DataAuto.RegisterNumber != "")
                            if (!generateProtocolWorker.IsBusy)
                                generateProtocolWorker.RunWorkerAsync(true);
                            else
                                MessageBox.Show("Podaj numer rejestracyjny auta w celu autoryzacji nowo pobranych pomiarów.");
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region DeleteMeasurementsCommand
        private RelayCommand _deleteMeasurementsCommand;
        public RelayCommand DeleteMeasurementsCommand
        {
            get
            {
                return _deleteMeasurementsCommand
                    ?? (_deleteMeasurementsCommand = new RelayCommand(
                    () =>
                    {
                        communicationWorker.RunWorkerAsync("DeleteMeasurementsFromCatalog");
                    },
                    () =>
                    {
                        // return IsConnectStatus;
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region EditAutoCommand
        private RelayCommand _editAutoCommand;
        public RelayCommand EditAutoCommand
        {
            get
            {
                return _editAutoCommand
                    ?? (_editAutoCommand = new RelayCommand(
                    () =>
                    {
                        if (IndexOfHistroicalDataGrid >= 0)
                        {
                            EditionView editionView = new EditionView();
                            SendDataToEditWindow();
                            editionView.ShowDialog();
                        }
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region GenerateHistoricalPDFCommand
        private RelayCommand _generateHistoricalPDFCommand;
        public RelayCommand GenerateHistoricalPDFCommand
        {
            get
            {
                return _generateHistoricalPDFCommand
                    ?? (_generateHistoricalPDFCommand = new RelayCommand(
                    () =>
                    {
                        if (IndexOfHistroicalDataGrid >= 0)
                        {
                            generateProtocolWorker.RunWorkerAsync(false);
                        }
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region DeleteAllMeasuremetsCommand
        private RelayCommand _deleteAllMeasuremetsCommand;
        public RelayCommand DeleteAllMeasuremetsCommand
        {
            get
            {
                return _deleteAllMeasuremetsCommand
                    ?? (_deleteAllMeasuremetsCommand = new RelayCommand(
                    () =>
                    {
                        MessageBoxResult result = MessageBox.Show("Czy chcesz usunąć wszytskie pomiary z pamięci z miernika?", "Wyczyszczenie historii", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                            communicationWorker.RunWorkerAsync("DeleteAllMeasurements");
                    },
                    () =>
                    {
                        // return IsConnectStatus;
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region SetFactorySettingsCommand
        private RelayCommand _setFactorySettingsCommand;
        public RelayCommand SetFactorySettingsCommand
        {
            get
            {
                return _setFactorySettingsCommand
                    ?? (_setFactorySettingsCommand = new RelayCommand(
                    () =>
                    {
                        communicationWorker.RunWorkerAsync("FactorySettings");
                    },
                    () =>
                    {
                        //return IsConnectStatus;
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region SetDateTimeFromComputerCommand
        private RelayCommand _setDateTimeFromComputerCommand;
        public RelayCommand SetDateTimeFromComputerCommand
        {
            get
            {
                return _setDateTimeFromComputerCommand
                    ?? (_setDateTimeFromComputerCommand = new RelayCommand(
                    () =>
                    {
                        communicationWorker.RunWorkerAsync("SetDateTimeFromComputer");
                    },
                    () =>
                    {
                        //return IsConnectStatus;
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region SetDateTimeFromCalendarCommand
        private RelayCommand _setDateTimeFromCalendarCommand;
        public RelayCommand SetDateTimeFromCalendarCommand
        {
            get
            {
                return _setDateTimeFromCalendarCommand
                    ?? (_setDateTimeFromCalendarCommand = new RelayCommand(
                    () =>
                    {
                        communicationWorker.RunWorkerAsync("SetDateTimeFromCalendar");
                    },
                    () =>
                    {
                        // return IsConnectStatus;
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region SearchAndSetPortMeterCommand
        private RelayCommand _searchAndSetPortMeterCommand;
        public RelayCommand SearchAndSetPortMeterCommand
        {
            get
            {
                return _searchAndSetPortMeterCommand
                    ?? (_searchAndSetPortMeterCommand = new RelayCommand(
                    () =>
                    {
                        if (!IsConnectStatus)
                        {
                            SearchAndConnectTimer.Start();
                            CheckExistingPortTimer.Stop();
                        }
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region SetManulaPortMeterCommand
        private RelayCommand _setManulaPortMeter;
        public RelayCommand SetManulaPortMeterCommand
        {
            get
            {
                return _setManulaPortMeter
                    ?? (_setManulaPortMeter = new RelayCommand(
                    () =>
                    {
                        if (!IsConnectStatus)
                            comunication.SetManualPort(PortsComItems[IndexOfPortsCom]);
                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region UpdateComItemsSource
        private RelayCommand _updateComItemsSourceCommand;
        public RelayCommand UpdateComItemsSourceCommand
        {
            get
            {
                return _updateComItemsSourceCommand
                    ?? (_updateComItemsSourceCommand = new RelayCommand(
                    () =>
                    {
                        PortsComItems = new ObservableCollection<string>(comunication.GetUSBDevices().Select(x => x.Name).ToList());
                    },
                    () =>
                    {

                        return true;
                    }
                    ));
            }
        }
        #endregion

        #region CloseWindowCommand
        private RelayCommand _closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand
                    ?? (_closeWindowCommand = new RelayCommand(
                    () =>
                    {
                        /// comunication.CloseConnect();
                    },
                    () =>
                    {

                        return true;
                    }
                    ));
            }
        }
        #endregion

        #endregion

        #region Timers

        private DispatcherTimer SearchAndConnectTimer;
        private DispatcherTimer CheckExistingPortTimer;

        private void InitializeCheckConnectTimer()
        {
            SearchAndConnectTimer = new DispatcherTimer();
            SearchAndConnectTimer.Tick += CheckConnectTimerTick;
            //SearchAndConnectTimer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.IntervalConnectTimer);
            SearchAndConnectTimer.Interval = new TimeSpan(0, 0, 0, 0, 800);
            if (Properties.Settings.Default.IsAutomaticSearchPort)
                SearchAndConnectTimer.Start();

        }

        private void CheckConnectTimerTick(object sender, EventArgs e)
        {
            if (!searchPortWorker.IsBusy)
                searchPortWorker.RunWorkerAsync();
        }

        public void InitializeCheckExistingPortTimer()
        {
            CheckExistingPortTimer = new DispatcherTimer();
            CheckExistingPortTimer.Tick += CheckExistingPortTimerTick;
            CheckExistingPortTimer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.IntervalCheckExistingPortTimer);
            CheckExistingPortTimer.Stop();
        }

        private void CheckExistingPortTimerTick(object sender, EventArgs e)
        {
            comunication.CheckExistingPort();
        }

        private void InitializeTimers()
        {
            InitializeCheckConnectTimer();
            InitializeCheckExistingPortTimer();
        }

        #endregion

        #region Workers

        private BackgroundWorker searchPortWorker;
        private BackgroundWorker checkExistingPortWorker;
        private BackgroundWorker measurementsWorker;
        private BackgroundWorker communicationWorker;
        private BackgroundWorker generateProtocolWorker;
        private BackgroundWorker openHistoricalDataWorker;
        private BackgroundWorker sendMessageToEditViewWorker;

        private void InitializeSearchPortWorker()
        {
            searchPortWorker = new BackgroundWorker();
            searchPortWorker.DoWork += SearchPort;

        }

        private void SearchPort(object sender, DoWorkEventArgs e)
        {
            comunication.SearchPortMeter();
            //searchPortWorker.Dispose();
        }

        private void InitializeCheckExistingPortWorker()
        {
            checkExistingPortWorker = new BackgroundWorker();
            checkExistingPortWorker.DoWork += CheckExistingPort;
        }

        private void CheckExistingPort(object sender, DoWorkEventArgs e)
        {
            comunication.CheckExistingPort();
        }

        private void InitializeSendMessageToEditViewWorker()
        {
            sendMessageToEditViewWorker = new BackgroundWorker();
            sendMessageToEditViewWorker.DoWork += OpenEditWindow;
        }

        private void OpenEditWindow(object sender, DoWorkEventArgs e)
        {
            SendDataToEditWindow();
        }

        private void InitializeMeasurementsWorker()
        {
            measurementsWorker = new BackgroundWorker();
            measurementsWorker.DoWork += GetMeasurements;
            measurementsWorker.RunWorkerCompleted += UpdateGetMeasurements;
        }

        private void UpdateGetMeasurements(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                ComunicationStatus = "Pobrano pomiary. Można wygenerować raport";
            }
            else
            {
                ComunicationStatus = "Katalog pusty";
            }
            IsBusy = false;
        }

        private void GetMeasurements(object sender, DoWorkEventArgs e)
        {
            ComunicationStatus = "";
            IsBusy = true;
            bool isGet = false;
            var measurementsCollection = comunication.ReadMeasuementsFromCatalog(IndexGetMeasurements);
            ItemSourceMeasurementsDataGrid = measurementsCollection;
            if (measurementsCollection.Count > 0)
                isGet = true;
            e.Result = isGet;
        }

        private void InitializeComunicationWorker()
        {
            communicationWorker = new BackgroundWorker();
            communicationWorker.DoWork += ComunicateToPaintMeter;
            communicationWorker.RunWorkerCompleted += UpdateComunicationStatus;
        }

        private void UpdateComunicationStatus(object sender, RunWorkerCompletedEventArgs e)
        {
            ComunicationStatus = e.Result.ToString();
            IsBusy = false;
        }

        private void ComunicateToPaintMeter(object sender, DoWorkEventArgs e)
        {
            ComunicationStatus = "";
            IsBusy = true;
            string message = (string)e.Argument;
            if (message == "FactorySettings")
                e.Result = comunication.SetFactorySettings();
            else if (message == "DeleteAllMeasurements")
                e.Result = comunication.DeleteAllMeasurements();
            else if (message == "DeleteMeasurementsFromCatalog")
                e.Result = comunication.DeleteMeasurements(IndexDeleteMeasurements);
            else if (message == "GetSerialNumber")
                e.Result = comunication.GetSerialNumber();
            else if (message == "SetDateTimeFromComputer")
                e.Result = comunication.SetDateTime(DateTime.Now);
            else if (message == "SetDateTimeFromCalendar")
                e.Result = comunication.SetDateTime(SetDateTime);
        }

        private void InitializeGenerateProtocolWorker()
        {
            generateProtocolWorker = new BackgroundWorker();
            generateProtocolWorker.DoWork += CreateProtocol;
        }

        private void CreateProtocol(object sender, DoWorkEventArgs e)
        {
            Thread threadGenerateProtocol;
            if ((bool)e.Argument)
            {
                serializer.SerializeCollectionMeasurements(ItemSourceMeasurementsDataGrid, DataAuto.RegisterNumber);
                SaveNewAuto();
                SaveNewDataContactDetails();
                SaveNewConclusions();
                threadGenerateProtocol = new Thread(new ThreadStart(GeneratePDFProtocol));
            }
            else
                threadGenerateProtocol = new Thread(new ThreadStart(GenerateHistoricalPDFProtocol));

            threadGenerateProtocol.ApartmentState = ApartmentState.STA;
            threadGenerateProtocol.Start();
        }

        private void InitializeOpenHistoricalDataWorker()
        {
            openHistoricalDataWorker = new BackgroundWorker();
            openHistoricalDataWorker.DoWork += OpenHistoricalData;
            openHistoricalDataWorker.RunWorkerCompleted += UpdateOpenHistoricalData;
        }

        private void UpdateOpenHistoricalData(object sender, RunWorkerCompletedEventArgs e)
        {
            ItemSourceHistoricalDataGrid = (ObservableCollection<Auto>)e.Result;
        }

        private void OpenHistoricalData(object sender, DoWorkEventArgs e)
        {
            e.Result = serializer.GetCollectionAutosFromFiles();
        }

        private void InitializeWorkers()
        {
            InitializeSearchPortWorker();
            InitializeCheckExistingPortWorker();
            InitializeMeasurementsWorker();
            InitializeComunicationWorker();
            InitializeGenerateProtocolWorker();
            InitializeOpenHistoricalDataWorker();
        }

        #endregion

        private void CreateDirectory()
        {
            string measurements = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.FolderOfMeasurements;
            string autos = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.FolderOfAuto;
            string dataContactDetails = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.FolderOfDataDetails;
            string conclusions = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.FolderOfConclusions;

            if (!Directory.Exists(measurements))
                Directory.CreateDirectory(measurements);
            if (!Directory.Exists(autos))
                Directory.CreateDirectory(autos);
            if (!Directory.Exists(dataContactDetails))
                Directory.CreateDirectory(dataContactDetails);
            if (!Directory.Exists(conclusions))
                Directory.CreateDirectory(conclusions);
            if (!Directory.Exists(conclusions))
            {
                if (!IsRunAsAdministrator())
                {
                    var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                    // The following properties run the new process as administrator
                    processInfo.UseShellExecute = true;
                    processInfo.Verb = "runas";

                    // Start the new process
                    try
                    {
                        Process.Start(processInfo);
                    }
                    catch (Exception)
                    {
                        // The user did not allow the application to run as administrator
                        MessageBox.Show("Aplikacja wymaga uruchomienia jakoadministrator przy pierwszym uruchomieniu");
                    }

                    // Shut down the current process
                    Application.Current.Shutdown();
                }
            }

        }
        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private void RegisterNotificationMessage()
        {
            Messenger.Default.Register<NotificationMessage<bool>>(this, NotificationMessageReceived);
            Messenger.Default.Register<NotificationMessage<string>>(this, NotificationMessageReceived);
        }

        private void SetConnectStatus(bool isConnect)
        {
            IsConnectStatus = isConnect;
            if (isConnect)
            {
                ConnectionStatus = "Połączono";
                ConnectButtonContent = "Połączony";
                SearchAndConnectTimer.Stop();
                CheckExistingPortTimer.Start();
            }
            else
            {
                ConnectionStatus = "Nie połączono";
                ConnectButtonContent = "Połącz";
                DataContactDetails.Model = "";
                DataContactDetails.Manufacturer = "";
                DataContactDetails.SerialNumber = "";
                if (IsCheckedAutomaticPortCom)
                {
                    CheckExistingPortTimer.Stop();
                    SearchAndConnectTimer.Start();
                }
            }
        }

        private void SendDataToEditWindow()
        {
            EditObject editObject = new EditObject();
            var auto = ItemSourceHistoricalDataGrid[IndexOfHistroicalDataGrid];
            editObject.collectionMeasurements = serializer.GetMeasurementsFromFile(auto.RegisterNumber);
            editObject.RegisterNumber = auto.RegisterNumber;
            editObject.ChassisNumber = auto.ChassisNumber;
            editObject.Model = auto.Model;
            editObject.Manufacturer = auto.Manufacturer;
            editObject.YearOfProduction = auto.YearOfProduction;
            editObject.Course = auto.Course;

            editObject.Conclusions = serializer.GetConclusionsFromFile(auto.RegisterNumber);

            var dataDetails = serializer.GetContactDetailsFromFile(auto.RegisterNumber);
            editObject.Name = dataDetails.Name;
            editObject.OtherData = dataDetails.OtherData;
            editObject.ModelOfProduct = dataDetails.ProductModel;
            editObject.ManufacturerOfProduct = dataDetails.ManufaturerProduct;
            editObject.NumberSerial = dataDetails.SerialNumberProduct;

            Messenger.Default.Send(new NotificationMessage<EditObject>(editObject, "SendEditionObject"));
        }

        private void SaveNewAuto()
        {
            Auto auto = new Auto();
            auto.RegisterNumber = DataAuto.RegisterNumber;
            auto.ChassisNumber = DataAuto.ChassisNumber;
            auto.Model = DataAuto.Model;
            auto.Manufacturer = DataAuto.Manufacturer;
            auto.YearOfProduction = DataAuto.YearOfProduction;
            auto.Course = DataAuto.Course;
            serializer.SerializeAuto(auto, DataAuto.RegisterNumber);
        }

        private void SaveNewDataContactDetails()
        {
            ContactDetails contactDetails = new ContactDetails();
            contactDetails.Name = DataContactDetails.Name;
            contactDetails.OtherData = DataContactDetails.Other;
            contactDetails.ProductModel = DataContactDetails.Model;
            contactDetails.ManufaturerProduct = DataContactDetails.Manufacturer;
            contactDetails.SerialNumberProduct = DataContactDetails.SerialNumber;

            serializer.SerializeDataDetails(contactDetails, DataAuto.RegisterNumber);
        }

        private void SaveNewConclusions()
        {
            serializer.SerializeConclusions(Conclusions, DataAuto.RegisterNumber);
        }

        private void GeneratePDFProtocol()
        {
            var auto = serializer.GetAutoFromFile(DataAuto.RegisterNumber);
            var dataContactDetails = serializer.GetContactDetailsFromFile(DataAuto.RegisterNumber);
            var conclusions = serializer.GetConclusionsFromFile(DataAuto.RegisterNumber);
            var measurements = serializer.GetMeasurementsFromFile(DataAuto.RegisterNumber);
            openHistoricalDataWorker.RunWorkerAsync();
            generator.CreateDocument(auto, dataContactDetails, measurements, conclusions);
            ItemSourceMeasurementsDataGrid = null;
            ComunicationStatus = "Wygenerowano raport";
            IsBusy = false;
        }

        private void GenerateHistoricalPDFProtocol()
        {
            if (IndexOfHistroicalDataGrid < ItemSourceHistoricalDataGrid.Count)
            {
                var auto = serializer.GetAutoFromFile(ItemSourceHistoricalDataGrid[IndexOfHistroicalDataGrid].RegisterNumber);
                var dataContactDetails = serializer.GetContactDetailsFromFile(ItemSourceHistoricalDataGrid[IndexOfHistroicalDataGrid].RegisterNumber);
                var conclusions = serializer.GetConclusionsFromFile(ItemSourceHistoricalDataGrid[IndexOfHistroicalDataGrid].RegisterNumber);
                var measurements = serializer.GetMeasurementsFromFile(ItemSourceHistoricalDataGrid[IndexOfHistroicalDataGrid].RegisterNumber);
                generator.CreateDocument(auto, dataContactDetails, measurements, conclusions);
            }
        }

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                });
            CreateDirectory();
            RegisterNotificationMessage();

            InitializeWorkers();
            InitializeTimers();
            openHistoricalDataWorker.RunWorkerAsync();
            //PortsComItems = comunication.SearchPortsInSystem();

            var v = comunication.GetUSBDevices();

            PortsComItems = new ObservableCollection<string>(comunication.GetUSBDevices().Select(x => x.Name).ToList());
        }

    }
}
using GalaSoft.MvvmLight.Messaging;
using NLog;
using PaintMeter.Classes.PDFGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PaintMeter.Classes
{
    public class Communications
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string serialPortName = "";
        private List<string> InadequateSerialPorts = new List<string>();

        private SerialPort CreateNewPort(string portName)
        {
            SerialPort serialPort = new SerialPort();

            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = 38400;
            serialPort.Handshake = Handshake.None;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 50;

            OpenPort(serialPort);
            return serialPort;
        }

        public void SetManualPort(string portName)
        {
            SendDefaultMessage(portName);
        }

        public void CloseConnect(SerialPort serialPort)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.WriteLine(PortMeterCommands.EndConnection);
                    ClosePort(serialPort);
                }
            }
            catch { }
        }

        private void OpenPort(SerialPort serialPort)
        {
            try
            {
                serialPort.Open();
            }
            catch { }
        }

        private void ClosePort(SerialPort serialPort)
        {
            try
            {
                serialPort.Close();
                serialPort.Dispose();
            }
            catch { }
        }

        public ObservableCollection<string> SearchPortsInSystem()
        {
            ObservableCollection<string> portsCollection = new ObservableCollection<string>();
            foreach (string portName in SerialPort.GetPortNames())
            {
                portsCollection.Add(portName);
            }
            return portsCollection;
        }

        public void ConnectToPort(string portName)
        {
            SendDefaultMessage(portName);
        }

        private void SendDefaultMessage(string portName)
        {
            try
            {
                SerialPort serialPort = CreateNewPort(portName);
                {
                    serialPort.Write(PortMeterCommands.DefaultMessage);
                    var receviedData = serialPort.ReadChar();
                    //if (receviedData == PortMeterCommands.ReceviedDefaultMessage)
                    //{
                    //    serialPort.Write(PortMeterCommands.GetSerialNumber);
                    //    UpdateSerialNumber(serialPort.ReadLine());
                    //    Properties.Settings.Default.PortCom = serialPort.PortName;
                    //    Properties.Settings.Default.Save();
                    //    serialPortName = portName;
                    //    UpdateConectStatus(true);
                    //}
                    //else
                    {
                        serialPort.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
        }

        public void SearchPortMeter()
        {
            var portsList = SerialPort.GetPortNames();
            foreach (string portName in portsList)
            {
                if(portName!="COM!")
                SendDefaultMessage(portName);
            }
        }

        public void CheckExistingPort()
        {
            bool isExisting = false;

            foreach (string portName in SerialPort.GetPortNames())
            {
                if (portName == serialPortName)
                    isExisting = true;
            }

            UpdateConectStatus(isExisting);
        }

        private void WriteByteToPortMeter(int sendNumber)
        {
            var serialPort = CreateNewPort(serialPortName);
            if (serialPort.IsOpen && serialPort != null)
            {
                try
                {
                    byte[] buffer = new byte[] { Convert.ToByte(sendNumber) };
                    serialPort.Write(buffer, 0, 1);
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ex);
                    MessageBox.Show(MessageToUser.writeToPortException);
                }
            }
        }

        public ObservableCollection<Measurement> ReadMeasuementsFromCatalog(int numberofCalatog)
        {
            ObservableCollection<Measurement> collecionMeasurements = new ObservableCollection<Measurement>();
            try
            {
                List<string> receviedList = new List<string>();
                var serialPort = CreateNewPort(serialPortName);
                serialPort.Write(PortMeterCommands.GetMeasurements);
                Thread.Sleep(200);
                WriteByteToPortMeter(numberofCalatog + 1);
                for (int i = 1; i < 101; i++)
                {
                    var receviedData = serialPort.ReadLine();
                    receviedList.Add(receviedData);
                }

                foreach (var rawValue in receviedList)
                {
                    var value = CreateMeasurementFromString(rawValue);
                    if (value.Value != "Nie zapisano")
                        collecionMeasurements.Add(value);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.writeToPortException);
            }
            return collecionMeasurements;
        }

        private Measurement CreateMeasurementFromString(string rawValue)
        {
            Measurement measurement = new Measurement();
            string[] separators = { ";", " " };
            string[] words = rawValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            measurement.Number = Convert.ToString(Convert.ToInt16(words[0]));
            measurement.Value = TemplatePDF.ReturnValue(Convert.ToInt16(words[1]));
            measurement.Stuff = TemplatePDF.ReturnStuff(Convert.ToInt16(words[2]));
            measurement.DateTimeMeasurement = Convert.ToString(words[3] + "." + words[4] + ".20" + words[5] + " " + words[6] + ":" + words[7] + ":" + words[8]);
            return measurement;
        }

        public string SetFactorySettings()
        {
            try
            {
                var serialPort = CreateNewPort(serialPortName);
                serialPort.Write(PortMeterCommands.FactorySettings);
                return "Ostatnia akcja: Przywrócono ustawienia fabryczne";
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.writeToPortException);
                return "Ostatnia akcja: " + MessageToUser.writeToPortException;
            }
        }

        public string DeleteAllMeasurements()
        {
            try
            {
                for (int i = 1; i < 11; i++)
                {
                    var serialPort = CreateNewPort(serialPortName);
                    serialPort.Write(PortMeterCommands.DeleteMeasurements);
                    Thread.Sleep(300);
                    WriteByteToPortMeter(i);
                    Thread.Sleep(500);
                }
                return "Ostatnia akcja: Usunięto pomiary z wszystkich katalogów";
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.writeToPortException);
                return "Ostatnia akcja: " + MessageToUser.writeToPortException;
            }
        }

        public string DeleteMeasurements(int catalog)
        {
            try
            {
                var serialPort = CreateNewPort(serialPortName);
                serialPort.Write(PortMeterCommands.DeleteMeasurements);
                Thread.Sleep(200);
                WriteByteToPortMeter((catalog + 1));
                return "Ostatnia akcja: Usunięto pomiary z katalogu " + (catalog + 1).ToString();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.writeToPortException);
                return "Ostatnia akcja: " + MessageToUser.writeToPortException;
            }
        }

        public string GetSerialNumber()
        {
            try
            {
                var serialPort = CreateNewPort(serialPortName);
                serialPort.Write(PortMeterCommands.GetSerialNumber);
                Thread.Sleep(200);
                var serialNumber = serialPort.ReadLine();
                return serialNumber;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.writeToPortException);
                return "";
            }
        }

        public string SetDateTime(DateTime dateTime)
        {
            try
            {
                var serialPort = CreateNewPort(serialPortName);
                serialPort.Write(PortMeterCommands.SetDateTime);
                Thread.Sleep(200);
                WriteByteToPortMeter(dateTime.Second + 1);
                Thread.Sleep(200);
                WriteByteToPortMeter(dateTime.Minute + 1);
                Thread.Sleep(200);
                WriteByteToPortMeter(dateTime.Hour + 1);
                Thread.Sleep(200);
                WriteByteToPortMeter(dateTime.Day + 1);
                Thread.Sleep(200);
                WriteByteToPortMeter(dateTime.Month + 1);
                Thread.Sleep(200);
                WriteByteToPortMeter((dateTime.Year % 100) + 1);
                Thread.Sleep(200);
                return "Ostatnia akcja: Zmieniono godzinę na " + dateTime.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.writeToPortException);
                return "Ostatnia akcja: " + MessageToUser.writeToPortException;
            }
        }

        private void UpdateConectStatus(bool isConect)
        {
            Messenger.Default.Send(new NotificationMessage<bool>(isConect, "ConnectStatus"));
        }

        private void UpdateSerialNumber(string serialNumber)
        {
            Messenger.Default.Send(new NotificationMessage<string>(serialNumber, "UpdateSerialNumber"));
        }
    }
}

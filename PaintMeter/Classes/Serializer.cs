using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace PaintMeter.Classes
{
    public class Serializer
    {
        private XmlSerializer serializer;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public void SerializeCollectionMeasurements(ObservableCollection<Measurement> collectionMeasurements, string fileName)
        {
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfMeasurements + fileName + ".xml";
                Stream saveStream = new FileStream(baseDirectory + Properties.Settings.Default.FolderOfMeasurements + fileName + ".xml", FileMode.Create, FileAccess.Write, FileShare.None);
                serializer = new XmlSerializer(typeof(ObservableCollection<Measurement>));
                serializer.Serialize(saveStream, collectionMeasurements);
                saveStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.saveException);
            }
        }

        public void SerializeAuto(Auto auto, string fileName)
        {
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfAuto + fileName + ".xml";
                Stream saveStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                serializer = new XmlSerializer(typeof(Auto));
                serializer.Serialize(saveStream, auto);
                saveStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.saveException);
            }
        }

        public void SerializeDataDetails(ContactDetails contactDetails, string fileName)
        {
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfDataDetails + fileName + ".xml";
                Stream saveStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                serializer = new XmlSerializer(typeof(ContactDetails));
                serializer.Serialize(saveStream, contactDetails);
                saveStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.saveException);
            }
        }

        public void SerializeConclusions(string conslusions, string fileName)
        {
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfConclusions + fileName + ".xml";
                Stream saveStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                serializer = new XmlSerializer(typeof(string));
                serializer.Serialize(saveStream, conslusions);
                saveStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.saveException);
            }
        }

        public ObservableCollection<Measurement> GetMeasurementsFromFile(string fileName)
        {
            ObservableCollection<Measurement> collecionMeasurements = new ObservableCollection<Measurement>();
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfMeasurements + fileName + ".xml";
                Stream openStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                serializer = new XmlSerializer(typeof(ObservableCollection<Measurement>));
                collecionMeasurements = serializer.Deserialize(openStream) as ObservableCollection<Measurement>;
                openStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.openException);
            }
            return collecionMeasurements;
        }

        public Auto GetAutoFromFile(string fileName)
        {
            Auto auto = new Auto();
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfAuto + fileName + ".xml";
                Stream openStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                serializer = new XmlSerializer(typeof(Auto));
                auto = serializer.Deserialize(openStream) as Auto;
                openStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.openException);
            }
            return auto;
        }

        public ContactDetails GetContactDetailsFromFile(string fileName)
        {
            ContactDetails contactDetails = new ContactDetails();
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfDataDetails + fileName + ".xml";
                Stream openStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                serializer = new XmlSerializer(typeof(ContactDetails));
                contactDetails = serializer.Deserialize(openStream) as ContactDetails;
                openStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.openException);
            }
            return contactDetails;
        }

        public ObservableCollection<Auto> GetCollectionAutosFromFiles()
        {
            ObservableCollection<Auto> collectionAutos = new ObservableCollection<Auto>();
            DirectoryInfo directoryInfo = new DirectoryInfo(baseDirectory + Properties.Settings.Default.FolderOfAuto);
            FileInfo[] filesAutos = directoryInfo.GetFiles("*.xml");
            foreach (FileInfo file in filesAutos)
            {
                collectionAutos.Add(GetAutoFromFile(Path.GetFileNameWithoutExtension(file.Name)));
            }
            return collectionAutos;
        }

        public string GetConclusionsFromFile(string fileName)
        {
            string conclusions = "";
            try
            {
                string path = baseDirectory + Properties.Settings.Default.FolderOfConclusions + fileName + ".xml";
                Stream openStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                serializer = new XmlSerializer(typeof(string));
                conclusions = serializer.Deserialize(openStream) as string;
                openStream.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.openException);
            }
            return conclusions;
        }

        public void DeleteFiles(string fileName)
        {
            string auto = baseDirectory + Properties.Settings.Default.FolderOfAuto + fileName + ".xml";
            string measurements = baseDirectory + Properties.Settings.Default.FolderOfMeasurements + fileName + ".xml";
            string conclusions = baseDirectory + Properties.Settings.Default.FolderOfConclusions + fileName + ".xml";
            string dataDetails = baseDirectory + Properties.Settings.Default.FolderOfDataDetails + fileName + ".xml";
            try
            {
                File.Delete(auto);
                File.Delete(measurements);
                File.Delete(conclusions);
                File.Delete(dataDetails);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                MessageBox.Show(MessageToUser.saveException);
            }

        }

        public Serializer()
        {

        }
    }
}

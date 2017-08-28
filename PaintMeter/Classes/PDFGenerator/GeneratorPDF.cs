using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PaintMeter.Classes.PDFGenerator
{
    public class GeneratorPDF
    {
        public void CreateDocument(Auto auto, ContactDetails contactDetails, ObservableCollection<Measurement> collectionMeasurements, string conclusions)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki PDF (*.pdf*)|*.pdf*";
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "Protokół pomiarowy auta " + auto.RegisterNumber;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);

                    Document document = new Document(PageSize.A4, TemplatePDF.LeftMargin, TemplatePDF.RightMargin, TemplatePDF.TopMargin, TemplatePDF.BottomMargin);
                    PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

                    document.Open();
                    SetMetaInformation(document);
                    AddImageToDocument(document);
                    AddTitleDocument(document);
                    CreateNextToOneAnotherTablesOnTop(document, contactDetails, auto);
                    document.Add(CreateMeasurementsTable(collectionMeasurements));
                    document.Add(CreateConslusions(conclusions));
                    document.Add(CreateSignature());
                    document.Close();
                    Process.Start(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Niepowodzenie zapisu pliku. Sprawdź czy nie nadpisujesz pliku o tej samej nazwie i czy nie masz go otworzonego");
                }
            }
        }

        private Document SetMetaInformation(Document document)
        {
            document.AddTitle("Protokół pomiarowy");
            document.AddSubject("Protokół pomiarowy grubości lakieru auta");
            document.AddKeywords("");
            document.AddCreator("TestTechnology");
            document.AddAuthor("TestTechnology");
            document.AddHeader("Nothing", "No Header");
            return document;
        }

        private void AddTitleDocument(Document document)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SpacingAfter = 10;
            paragraph.SpacingBefore = 10;

            paragraph.Add(new Phrase(new Chunk("PROTOKÓŁ POMIAROWY", TemplatePDF.titleFontContactDetailsTable)));
            document.Add(paragraph);
        }

        private Document CreateNextToOneAnotherTablesOnTop(Document document, ContactDetails contactDetails, Auto auto)
        {
            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;
            table.AddCell(CreateAutoDataTable(auto));
            table.AddCell(CreateContactDetailsTable(contactDetails));
            document.Add(table);
            return document;
        }

        private PdfPTable CreateContactDetailsTable(ContactDetails contactDetails)
        {
            float[] widthsCollumns = new float[] { 1, 2 };
            PdfPTable table = new PdfPTable(TemplatePDF.NumberOfColumnsContactDetailsTable);
            table.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Left;
            table.TotalWidth = 250;
            table.LockedWidth = true;
            table.SetWidths(widthsCollumns);
            table.SpacingBefore = 20;

            Chunk title = new Chunk("Dane przeprowadzającego pomiar", TemplatePDF.titleFontContactDetailsTable);
            Chunk titleName = new Chunk("Imię i nazwisko: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleOtherData = new Chunk("Pozostałe dane: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk title2 = new Chunk("Dane urządzenia pomiarowego", TemplatePDF.titleFontContactDetailsTable);
            Chunk titleModel = new Chunk("Model: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleManufacturer = new Chunk("Producent: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleSerialNumber = new Chunk("Nr seryjny: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk name = new Chunk(contactDetails.Name, TemplatePDF.cellFontValueTable);
            Chunk otherData = new Chunk(contactDetails.OtherData, TemplatePDF.cellFontValueTable);
            Chunk model = new Chunk(contactDetails.ProductModel, TemplatePDF.cellFontValueTable);
            Chunk manufacturer = new Chunk(contactDetails.ManufaturerProduct, TemplatePDF.cellFontValueTable);
            Chunk serialNumber = new Chunk(contactDetails.SerialNumberProduct, TemplatePDF.cellFontValueTable);

            PdfPCell cell = new PdfPCell(new Phrase(title));
            cell.Colspan = 2;
            cell.MinimumHeight = 20;
            cell.VerticalAlignment = (int)VerticalAlignment.Top;
            cell.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Center;
            PdfPCell cell2 = new PdfPCell(new Phrase(title2));
            cell2.Colspan = 2;
            cell2.MinimumHeight = 20;
            cell2.VerticalAlignment = (int)VerticalAlignment.Top;
            cell2.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Center;

            table.AddCell(cell);
            table.AddCell(new PdfPCell(new Phrase(titleName)));
            table.AddCell(new PdfPCell(new Phrase(name)));
            table.AddCell(new PdfPCell(new Phrase(titleOtherData)));
            table.AddCell(new PdfPCell(new Phrase(otherData)));
            table.AddCell(cell2);
            table.AddCell(new PdfPCell(new Phrase(titleModel)));
            table.AddCell(new PdfPCell(new Phrase(model)));
            table.AddCell(new PdfPCell(new Phrase(titleManufacturer)));
            table.AddCell(new PdfPCell(new Phrase(manufacturer)));
            table.AddCell(new PdfPCell(new Phrase(titleSerialNumber)));
            table.AddCell(new PdfPCell(new Phrase(serialNumber)));

            return table;
        }

        private PdfPTable CreateAutoDataTable(Auto auto)
        {
            float[] widthsCollumns = new float[] { 1, 2 };
            PdfPTable table = new PdfPTable(TemplatePDF.NumberOfColumnsContactDetailsTable);
            table.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Right;
            table.TotalWidth = 250;
            table.LockedWidth = true;
            table.SetWidths(widthsCollumns);
            table.SpacingBefore = 20;

            Chunk title = new Chunk("Dane badanego pojazdu", TemplatePDF.titleFontContactDetailsTable);
            Chunk titleRegisterNumber = new Chunk("Nr rejestracyjny: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleChassisNumber = new Chunk("Numer VIN: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleModel = new Chunk("Model: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleManufacturer = new Chunk("Producent: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleYearOfProduction = new Chunk("Rok produkcji: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleCourse = new Chunk("Przebieg: ", TemplatePDF.cellTitleFontContactDetailsTable);

            Chunk registerNumber = new Chunk(auto.RegisterNumber, TemplatePDF.cellFontValueTable);
            Chunk chassisNumber = new Chunk(auto.ChassisNumber, TemplatePDF.cellFontValueTable);
            Chunk model = new Chunk(auto.Model, TemplatePDF.cellFontValueTable);
            Chunk manufacturer = new Chunk(auto.Manufacturer, TemplatePDF.cellFontValueTable);
            Chunk yearOfProduction = new Chunk(auto.YearOfProduction, TemplatePDF.cellFontValueTable);
            Chunk course = new Chunk(auto.Course, TemplatePDF.cellFontValueTable);

            PdfPCell cell = new PdfPCell(new Phrase(title));
            cell.Colspan = 2;
            cell.MinimumHeight = 20;
            cell.VerticalAlignment = (int)VerticalAlignment.Top;
            cell.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Center;

            table.AddCell(cell);
            table.AddCell(new PdfPCell(new Phrase(titleRegisterNumber)));
            table.AddCell(new PdfPCell(new Phrase(registerNumber)));
            table.AddCell(new PdfPCell(new Phrase(titleChassisNumber)));
            table.AddCell(new PdfPCell(new Phrase(chassisNumber)));
            table.AddCell(new PdfPCell(new Phrase(titleModel)));
            table.AddCell(new PdfPCell(new Phrase(model)));
            table.AddCell(new PdfPCell(new Phrase(titleManufacturer)));
            table.AddCell(new PdfPCell(new Phrase(manufacturer)));
            table.AddCell(new PdfPCell(new Phrase(titleYearOfProduction)));
            table.AddCell(new PdfPCell(new Phrase(yearOfProduction)));
            table.AddCell(new PdfPCell(new Phrase(titleCourse)));
            table.AddCell(new PdfPCell(new Phrase(course)));

            return table;
        }

        private PdfPTable CreateMeasurementsTable(ObservableCollection<Measurement> collectionMeasurements)
        {
            float[] widthsCollumns = new float[] { 1, 2, 2, 4, 4 };
            PdfPTable table = new PdfPTable(5);
            table.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Center;
            table.TotalWidth = 500;
            table.LockedWidth = true;
            table.SetWidths(widthsCollumns);
            table.SpacingAfter = 20;
            table.SpacingBefore = 20;

            Chunk title = new Chunk("Tabela pomiarowa", TemplatePDF.titleFontContactDetailsTable);
            Chunk titleLp = new Chunk("Lp.: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleValue = new Chunk("Wartość: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleStuff = new Chunk("Materiał: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleDateTime = new Chunk("Data pomiaru / godzina: ", TemplatePDF.cellTitleFontContactDetailsTable);
            Chunk titleGraphicPresentation = new Chunk("Graficzna prezentacja: ", TemplatePDF.cellTitleFontContactDetailsTable);

            PdfPCell cell = new PdfPCell(new Phrase(title));
            cell.Colspan = 5;
            cell.MinimumHeight = 20;
            cell.VerticalAlignment = (int)VerticalAlignment.Top;
            cell.HorizontalAlignment = (int)System.Windows.HorizontalAlignment.Center;
            table.AddCell(cell);

            table.AddCell(new PdfPCell(new Phrase(titleLp)));
            table.AddCell(new PdfPCell(new Phrase(titleValue)));
            table.AddCell(new PdfPCell(new Phrase(titleStuff)));
            table.AddCell(new PdfPCell(new Phrase(titleDateTime)));
            table.AddCell(new PdfPCell(new Phrase(titleGraphicPresentation)));

            for (int i = 0; i < collectionMeasurements.Count; i++)
            {
                table.AddCell(new PdfPCell(new Phrase(new Chunk((i + 1).ToString() + ".", TemplatePDF.cellFontValueTable))));
                table.AddCell(new PdfPCell(GetValueCell(collectionMeasurements[i].Value)));
                table.AddCell(new PdfPCell(new Phrase(new Chunk(collectionMeasurements[i].Stuff, TemplatePDF.cellFontValueTable))));
                table.AddCell(new PdfPCell(new Phrase(new Chunk(TemplatePDF.ReturnDateTimeMeasurement(collectionMeasurements[i].DateTimeMeasurement), TemplatePDF.cellFontValueTable))));
                table.AddCell(new PdfPCell(GetGraphcialPresentation(collectionMeasurements[i].Value)));
            }
            return table;
        }

        private Paragraph GetGraphcialPresentation(string value)
        {
            Paragraph paragraph = new Paragraph();
            var chunk = new Chunk(TemplatePDF.ReturnGraphicalPresentation(value), TemplatePDF.graphicalPresentation);
            chunk.SetCharacterSpacing(-1);
            paragraph.Add(chunk);
            return paragraph;
        }

        private Paragraph GetValueCell(string value)
        {
            Paragraph measurementsValueParagraph = new Paragraph();
            measurementsValueParagraph.Add(new Chunk(value, TemplatePDF.cellFontValueTable));
            if (value != "Nie zapisano" && value != "--------")
            {
                measurementsValueParagraph.Add(new Chunk(" μm", TemplatePDF.specialSymbolFont));
                measurementsValueParagraph.Add(new Chunk("m", TemplatePDF.cellFontValueTable));
            }
            return measurementsValueParagraph;
        }

        private Paragraph CreateConslusions(string conclusions)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SpacingAfter = 10;
            paragraph.SpacingBefore = 40;
            paragraph.Add(new Phrase(new Chunk("Wnioski pomiarowe:", TemplatePDF.cellTitleFontContactDetailsTable)));
            paragraph.Add(new Phrase(new Chunk(conclusions, TemplatePDF.cellFontValueTable)));
            return paragraph;
        }

        private Paragraph CreateSignature()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SpacingAfter = 20;
            paragraph.SpacingBefore = 40;
            paragraph.Add(new Phrase(new Chunk("Podpis :", TemplatePDF.cellTitleFontContactDetailsTable)));
            return paragraph;
        }

        private Document AddImageToDocument(Document document)
        {
            BitmapImage glowIcon = new BitmapImage();
            glowIcon.BeginInit();
            glowIcon.UriSource = new Uri("pack://application:,,,/PaintMeter;component/Resources\\MiernikLakieru.ico");
            glowIcon.EndInit();

            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(glowIcon));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            Image image = Image.GetInstance(data); //Image.GetInstance(TemplatePDF.LogoPath);
            image.ScalePercent(TemplatePDF.ScaledLogoPercent);
            document.Add(image);
            return document;
        }
    }
}

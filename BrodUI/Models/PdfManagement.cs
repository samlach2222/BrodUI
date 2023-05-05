using BrodUI.Helpers;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Canvas = iText.Layout.Canvas;
using File = System.IO.File;
using Path = System.IO.Path;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage the PDF file
    /// </summary>
    public class PdfManagement
    {
        /// <summary>
        /// Path of the PDF file
        /// </summary>
        private string? PdfPath { get; set; }

        /// <summary>
        /// Document of the PDF file
        /// </summary>
        private PdfDocument? Document { get; set; }

        /// <summary>
        /// List of wires passed in the constructor
        /// </summary>
        private List<Wire> WiresList { get; set; }

        /// <summary>
        /// Image to insert in the PDF file
        /// </summary>
        private BitmapImage? Image { get; set; }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="wiresList">List of wires to display it in the second page</param>
        public PdfManagement(List<Wire> wiresList)
        {
            WiresList = wiresList;
            UserChooseWhereToSaveThePdfFile();
        }

        /// <summary>
        /// Function to create all pages of the PDF file and add them to the document
        /// - The first page have the title of the file itself, the date and time, and the image
        /// - The second page is the list of the wires and the quantity for each
        /// - The third page is the embroidery pattern
        /// </summary>
        private void CreatePdfDocument()
        {
            Document = new PdfDocument(new PdfWriter(PdfPath));
            // add 3 pages
            Document.AddNewPage();
            Document.AddNewPage();
            Document.AddNewPage();

            // Create pages
            CreateFirstPage();
            CreateSecondPage();
            CreateThirdPage();
            InsertFooter();

            // Close the document
            Document.Close();

            // Open the PDF file
            OpenPdfInDefaultApplication();
        }

        /// <summary>
        /// Function to insert a footer in each page in the PDF document
        /// The footer contains the page number and the watermark
        /// </summary>
        private void InsertFooter()
        {
            for (int i = 1; i <= 3; i++)
            {
                PdfPage page = Document!.GetPage(i);
                PdfCanvas canvas = new(page);
                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                const float fontSize = 8;
                string text = "Page " + i + " of 3";
                float textWidth = font.GetWidth(text, fontSize);
                float textX = page.GetPageSize().GetWidth() - (textWidth + 30);
                float textY = page.GetPageSize().GetHeight() - (page.GetPageSize().GetHeight() / 1.03f);
                canvas.BeginText()
                    .SetFontAndSize(font, fontSize)
                    .MoveText(textX, textY)
                    .ShowText(text)
                    .EndText();
                canvas.Release();

                PdfCanvas waterMark = new(page);
                const string wm = "Created with BrodUI";
                const float textXWm = 30;
                float textYWm = page.GetPageSize().GetHeight() - (page.GetPageSize().GetHeight() / 1.03f);
                waterMark.BeginText()
                    .SetFontAndSize(font, fontSize)
                    .MoveText(textXWm, textYWm)
                    .ShowText(wm)
                    .EndText();
                waterMark.Release();
            }
        }

        /// <summary>
        /// Function to allow the user to choose where to save the PDF file and the name of the file
        /// </summary>
        private void UserChooseWhereToSaveThePdfFile()
        {
            // Create a SaveFileDialog
            SaveFileDialog saveFileDialog = new()
            {
                // Set the default file name
                FileName = "BrodUI_TempName",
                // Set the default extension
                DefaultExt = ".pdf",
                // Set the filter
                Filter = "PDF documents (.pdf)|*.pdf"
            };
            // Show the dialog
            bool? result = saveFileDialog.ShowDialog();
            // If the user click on "Save"
            if (result != true) return;
            // Get the path of the file
            PdfPath = saveFileDialog.FileName;
            CreatePdfDocument();
        }

        /// <summary>
        /// Function to create the first page of the PDF file
        /// The first page contain the title of the file itself, the date and time, and the image
        /// </summary>
        private void CreateFirstPage()
        {
            // The first page have the title of the file itself, the date and time, and the image
            // Get the first page
            PdfPage page = Document!.GetPage(1);

            // ------------------------ //
            //     TITLE OF THE FILE    //
            // ------------------------ //

            PdfCanvas titleCanvas = new(page);
            // Get the title of the PDF file
            if (PdfPath != null)
            {
                string[] temp = PdfPath.Split('\\');
                string title = temp[^1].Split('.')[0];
                // Get the font
                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                // Get the size of the font
                float fontSize = 40;
                // Get the width of the title
                float titleWidth = font.GetWidth(title, fontSize);
                // Get the position of the title
                float titleX = (page.GetPageSize().GetWidth() - titleWidth) / 2;
                float titleY = page.GetPageSize().GetHeight() - (page.GetPageSize().GetHeight() / 9);
                // Display the title
                titleCanvas.BeginText()
                    .SetFontAndSize(font, fontSize)
                    .MoveText(titleX, titleY)
                    .ShowText(title)
                    .EndText();
            }

            titleCanvas.Release();

            // ------------------------ //
            //         DATE TIME        //
            // ------------------------ //

            PdfCanvas dateTimeCanvas = new(page);
            // Get the title of the file
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            // Get the font
            PdfFont fontDateTime = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            // Get the size of the font
            float fontSizeDateTime = 12f;
            // Get the width of the title
            float dateTimeWidth = fontDateTime.GetWidth(dateTime, fontSizeDateTime);
            // Get the position of the title
            float dateTimeX = (page.GetPageSize().GetWidth() - dateTimeWidth) / 2;
            float dateTimeY = page.GetPageSize().GetHeight() - (page.GetPageSize().GetHeight() / 7);
            // Display the title
            dateTimeCanvas.BeginText()
                .SetFontAndSize(fontDateTime, fontSizeDateTime)
                .MoveText(dateTimeX, dateTimeY)
                .ShowText(dateTime)
                .EndText();
            dateTimeCanvas.Release();

            // ------------------------ //
            //           IMAGE          //
            // ------------------------ //

            // Get the canvas of the page
            PdfCanvas imgCanvas = new(page);

            // Display the image in the center of the page
            // Get the image from the path
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(appData + "\\BrodUI"))
            {
                Directory.CreateDirectory(appData + "\\BrodUI");
            }
            // path combine with the file name
            string imgPath = Path.Combine(appData + "\\BrodUI", "current image.png");
            if (File.Exists(imgPath))
            {
                // create a temp copy of the image with specified width and height
                // Get the image from the path
                Image = new(new Uri(imgPath));
                // Create a new image with the specified width and height
                double minWidth = Document.GetDefaultPageSize().GetWidth() / 2;
                double minHeight = Document.GetDefaultPageSize().GetHeight() / 2;
                // Get the width and height of the image
                double width = Image.Width;
                double height = Image.Height;
                // Get the ratio of the image
                double ratio = width / height;
                // Get the new width and height of the image
                double newWidth = width;
                double newHeight = height;
                // check if the image is vertical or horizontal
                if (width > height)
                {
                    // if the image is horizontal
                    if (width < minWidth)
                    {
                        newWidth = minWidth;
                        newHeight = newWidth / ratio;
                    }
                }
                else
                {
                    // if the image is vertical
                    if (height < minHeight)
                    {
                        newHeight = minHeight;
                        newWidth = newHeight * ratio;
                    }
                }
                // Create a new image with the new width and height
                BitmapImage newImg = new();
                newImg.BeginInit();
                newImg.UriSource = new Uri(imgPath);
                newImg.DecodePixelWidth = (int)newWidth;
                newImg.DecodePixelHeight = (int)newHeight;
                newImg.EndInit();
                // Save the new image
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(newImg));
                string tempPath = Path.GetTempPath(); // get temp folder
                string tempFile = Path.Combine(tempPath, "temp.png"); // create a temp file
                using FileStream fileStream = new(tempFile, FileMode.Create); // save the image
                encoder.Save(fileStream);
                fileStream.Close();

                // Get the image data
                ImageData imageData = ImageDataFactory.Create(tempFile);

                // Get X and Y coordinates to have the image in the center of the page
                float x = (Document.GetDefaultPageSize().GetWidth() / 2) - (imageData.GetWidth() / 2);
                float y = (Document.GetDefaultPageSize().GetHeight() / 2) - (imageData.GetHeight() / 2);

                // Add the image to the canvas
                imgCanvas.AddImageAt(imageData, x, y, true);

                // Delete the temp file
                File.Delete(tempFile);
            }
            imgCanvas.Release();
        }

        /// <summary>
        /// Function to create the second page of the PDF file
        /// This page contains the list of wires
        /// </summary>
        private void CreateSecondPage()
        {
            // Get the second page
            PdfPage page = Document!.GetPage(2);

            // ------------------------ //
            //     TITLE OF THE PAGE    //
            // ------------------------ //

            PdfCanvas titleCanvas = new(page);
            const string title = "List of wires";
            // Get the font
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            // Get the size of the font
            const float fontSize = 30;
            // Get the width of the title
            float titleWidth = font.GetWidth(title, fontSize);
            // Get the position of the title
            float titleX = (page.GetPageSize().GetWidth() - titleWidth) / 2;
            float titleY = page.GetPageSize().GetHeight() - (page.GetPageSize().GetHeight() / 7);
            // Display the title
            titleCanvas.BeginText()
                .SetFontAndSize(font, fontSize)
                .MoveText(titleX, titleY)
                .ShowText(title)
                .EndText();
            titleCanvas.Release();

            // ------------------------ //
            //       TABLE OF WIRES     //
            // ------------------------ //
            // Add  table to the canvas
            Table table = new(5);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            // Add the header for the 5 columns
            Cell headerColor = new();
            headerColor.Add(new Paragraph("Color"));
            headerColor.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            headerColor.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            headerColor.SetTextAlignment(TextAlignment.CENTER);
            headerColor.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(headerColor);

            Cell headerNumber = new();
            headerNumber.Add(new Paragraph("Number"));
            headerNumber.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            headerNumber.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            headerNumber.SetTextAlignment(TextAlignment.CENTER);
            headerNumber.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(headerNumber);

            Cell headerType = new();
            headerType.Add(new Paragraph("Type"));
            headerType.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            headerType.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            headerType.SetTextAlignment(TextAlignment.CENTER);
            headerType.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(headerType);

            Cell headerName = new();
            headerName.Add(new Paragraph("Name"));
            headerName.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            headerName.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            headerName.SetTextAlignment(TextAlignment.CENTER);
            headerName.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(headerName);

            Cell headerQuantity = new();
            headerQuantity.Add(new Paragraph("Quantity"));
            headerQuantity.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            headerQuantity.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            headerQuantity.SetTextAlignment(TextAlignment.CENTER);
            headerQuantity.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(headerQuantity);

            // Add the data for each wire
            foreach (Wire wire in WiresList)
            {
                // Convert Color
                BrushConverter converter = new();
                SolidColorBrush brush = (SolidColorBrush)converter.ConvertFromString(wire.Color.ToString())!;
                iText.Kernel.Colors.Color color = new DeviceRgb(brush.Color.R, brush.Color.G, brush.Color.B);

                Div rectangle = new Div()
                    .SetHeight(15)
                    .SetWidth(15)
                    .SetBackgroundColor(color)
                    .SetBorder(new SolidBorder(ColorConstants.BLACK, 1));

                Cell colorCell = new();
                colorCell.Add(rectangle);
                colorCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                colorCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                colorCell.SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(colorCell);

                Cell numberCell = new();
                numberCell.Add(new Paragraph(wire.Number.ToString()));
                numberCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                numberCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                numberCell.SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(numberCell);

                Cell typeCell = new();
                typeCell.Add(new Paragraph(wire.Type));
                typeCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                typeCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                typeCell.SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(typeCell);

                Cell nameCell = new();
                nameCell.Add(new Paragraph(wire.Name));
                nameCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                nameCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                nameCell.SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(nameCell);

                Cell quantityCell = new();
                quantityCell.Add(new Paragraph(wire.Quantity.ToString()));
                quantityCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                quantityCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                quantityCell.SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(quantityCell);
            }

            PdfCanvas tableCanvas = new(Document.GetPage(2));
            Rectangle canvasArea = new(20, 40, page.GetPageSize().GetWidth() - 40, page.GetPageSize().GetHeight() * 6 / 8);
            Canvas canvas = new(tableCanvas, canvasArea);
            canvas.Add(table);
            canvas.Close();

            // Add the table to the canvas 
            tableCanvas.Release();
        }

        /// <summary>
        /// Function to create the third page of the PDF
        /// This page contains the image of the embroidery
        /// </summary>
        private void CreateThirdPage()
        {
            // Get the second page
            PdfPage page = Document!.GetPage(3);

            // ------------------------ //
            //     TITLE OF THE PAGE    //
            // ------------------------ //
            int imgWidth = Image!.PixelWidth;
            int imgHeight = Image!.PixelHeight;

            Brush[,] wireTable = ImageToDataTable.ConvertTo2dArray(Image!);

            int cellSize = int.Parse(ConfigManagement.GetEmbroiderySizeFromConfigFile()!);

            Table table = new(imgWidth);
            table.SetWidth(cellSize * imgWidth);
            table.SetHeight(cellSize * imgHeight);

            // add each pixel of the wireTable to the table
            for (int i = 0; i < imgHeight; i++)
            {
                for (int j = 0; j < imgWidth; j++)
                {
                    // Convert Color
                    BrushConverter converter = new();
                    SolidColorBrush brush = (SolidColorBrush)converter.ConvertFromString(wireTable[j, i].ToString())!;
                    iText.Kernel.Colors.Color color = new DeviceRgb(brush.Color.R, brush.Color.G, brush.Color.B);
                    Cell cell = new();
                    cell.SetBackgroundColor(color);

                    // borders
                    float borderLeft = 0.5f;
                    const float borderRight = 0.5f;
                    float borderTop = 0.5f;
                    const float borderBottom = 0.5f;
                    if (j > 0)
                    {
                        if (j % 5 == 0)
                        {
                            borderLeft += 1;
                        }

                        if (j % 10 == 0)
                        {
                            borderLeft += 1;
                        }
                    }

                    if (i > 0)
                    {
                        if (i % 5 == 0)
                        {
                            borderTop += 1;
                        }
                        if (i % 10 == 0)
                        {
                            borderTop += 1;
                        }
                    }

                    // create border
                    cell.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, borderLeft));
                    cell.SetBorderRight(new SolidBorder(ColorConstants.BLACK, borderRight));
                    cell.SetBorderTop(new SolidBorder(ColorConstants.BLACK, borderTop));
                    cell.SetBorderBottom(new SolidBorder(ColorConstants.BLACK, borderBottom));

                    table.AddCell(cell);
                }
            }

            // adjust size of border in the table


            PdfCanvas tableCanvas = new(Document.GetPage(3));

            float startPointX = (page.GetPageSize().GetWidth()) / 2 - cellSize * imgWidth / 2f;
            float startPointY = (page.GetPageSize().GetHeight()) / 2 - cellSize * imgHeight / 2f;

            Rectangle canvasArea = new(startPointX, startPointY, cellSize * imgWidth, cellSize * imgHeight);
            Canvas canvas = new(tableCanvas, canvasArea);
            canvas.Add(table);
            canvas.Close();

            // Add the table to the canvas 
            tableCanvas.Release();
        }

        /// <summary>
        /// Function to open the PDF in the default application
        /// </summary>
        public void OpenPdfInDefaultApplication()
        {
            if (PdfPath == null) return;
            new Process
            {
                StartInfo = new ProcessStartInfo(PdfPath!)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}

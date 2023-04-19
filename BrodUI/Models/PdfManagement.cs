using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace BrodUI.Models
{
    public class PdfManagement
    {
        public string? PdfPath { get; set; }

        public PdfDocument? Document { get; set; }

        public PdfManagement()
        {
            UserChooseWhereToSaveThePdfFile();
        }

        private void CreatePdfDocument()
        {
            Document = new PdfDocument(new PdfWriter(PdfPath));
            // add 3 pages
            Document.AddNewPage();
            Document.AddNewPage();
            Document.AddNewPage();

            // Create the first page
            CreateFirstPage();
            InsertFooter();

            // Close the document
            Document.Close();
        }

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
            float fontSizeDateTime = 12;
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
                BitmapImage img = new(new Uri(imgPath));
                // Create a new image with the specified width and height
                double minWidth = Document.GetDefaultPageSize().GetWidth() / 2;
                double minHeight = Document.GetDefaultPageSize().GetHeight() / 2;
                // Get the width and height of the image
                double width = img.Width;
                double height = img.Height;
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
    }

    /*
     * DESIGN
     *
     * - The first page have the title of the file itself, the date and time, and the image
     * - The second page is the list of the wires and the quantity for each
     * - The third page is the embroidery pattern
     */


    /*
     * HELPER
     *
     * - To add a new page : Document.AddNewPage();
     * - To add a new page at a specific index : Document.AddNewPage(index);
     * - To get the number of pages : Document.GetNumberOfPages();
     * - To get the page at a specific index : Document.GetPage(index);
     * - To get the size of the page : Document.GetDefaultPageSize();
     * - To set the size of the page : Document.SetDefaultPageSize(size);
     * - To display a text :
     *                      PdfCanvas canvas = new(Document.<ThePageYouWant>);
     *                      canvas.BeginText();
     *                      canvas.SetFontAndSize(PdfFontFactory.CreateFont(), <FontSize>);
     *                      canvas.MoveText(<CoordX>, <CoordY>);
     *                      canvas.ShowText(<Your Text>);
     *                      canvas.EndText();
     *                      canvas.Release();
     * - To display a table : Table table = new(<NumberOfColumns>);
     *                      
     */
}

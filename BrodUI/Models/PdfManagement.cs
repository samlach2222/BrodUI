using iText.Kernel.Pdf;
using System;
using System.IO;
using Path = System.IO.Path;

namespace BrodUI.Models
{
    public class PdfManagement
    {
        public string PdfPath { get; set; }

        public PdfDocument Document { get; set; }

        public PdfManagement()
        {
            // get Appdata Roaming folder in a string
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(appData + "\\BrodUI"))
            {
                Directory.CreateDirectory(appData + "\\BrodUI");
            }
            // path combine with the file name
            PdfPath = Path.Combine(appData + "\\BrodUI", "BrodUI.pdf");
            if (!File.Exists(PdfPath))
            {
                Document = new PdfDocument(new PdfWriter(PdfPath));
                // add the first page
                Document.AddNewPage();
                // add the second page
                Document.AddNewPage();
                // add the third page
                Document.AddNewPage();
            }
            else
            {
                // Get the document from the path
                Document = new PdfDocument(new PdfReader(PdfPath), new PdfWriter(PdfPath));
            }
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

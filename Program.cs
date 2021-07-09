//	Copyright © 2021, EPSITEC SA, CH-1400 Yverdon-les-Bains, Switzerland
//	Author: Pierre ARNAUD, Maintainer: Pierre ARNAUD

using Spire.Pdf;
using Spire.Pdf.Print;

using System.Drawing.Printing;

namespace Epsitec.Rnd
{
    class Program
    {
        static void Main()
        {
            var pdfData = System.IO.File.ReadAllBytes ("multipage.pdf");

            //  My default printer has multiple trays...

            //  PaperSource #3 = External paper tray
            //  PaperSource #4 = Main paper tray

            void HandlePaperSettingsA(object sender, PdfPaperSettingsEventArgs e)
            {
                var page = e.CurrentPaper;
                var tray = page switch
                {
                    2 => 3,
                    _ => 4,
                };

                e.CurrentPaperSource = e.PaperSources[tray];
            }
            
            void HandlePaperSettingsB(object sender, PdfPaperSettingsEventArgs e)
            {
                var page = e.CurrentPaper;
                var tray = page switch
                {
                    2 => 4,
                    _ => 3,
                };

                e.CurrentPaperSource = e.PaperSources[tray];
            }

            //  Should print pages 1 and 3 on source #4, page 2 on source #3
            //  Everything gets printed on source #4

            {
                using var doc = new PdfDocument (pdfData);

                doc.PrintSettings.PrintController = new StandardPrintController ();
                doc.PrintSettings.PaperSettings += HandlePaperSettingsA;
                doc.PrintSettings.Duplex = Duplex.Simplex;

                doc.Print ();
            }

            //  Should print pages 1 and 3 on source #3, page 2 on source #4
            //  Everything gets printed on source #3

            {
                using var doc = new PdfDocument (pdfData);

                doc.PrintSettings.PrintController = new StandardPrintController ();
                doc.PrintSettings.PaperSettings += HandlePaperSettingsB;
                doc.PrintSettings.Duplex = Duplex.Simplex;

                doc.Print ();
            }
        }
    }
}

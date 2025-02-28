using iText.Kernel.Pdf;
using iText.Layout.Element;
using System.Reflection.Metadata;
using TPS.Application.Abstractions;
using iText.Layout;
using iText.IO.Image;
using iText.Layout.Properties;
using System.IO;
namespace TPS.Application.Services
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> GeneratePdf(string content)
        {
            //string logoPath = "logo.png"; // Ensure the logo file exists
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                // Add Logo
                //Image logo = new Image(ImageDataFactory.Create(logoPath)).SetWidth(100);
                //document.Add(logo);

                // Add Title
                Paragraph title = new Paragraph("Report Title")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(24)
                    .SetBold();
                document.Add(title);

                // Add Table
                Table table = new Table(new float[] { 1, 3, 2 }).UseAllAvailableWidth();
                table.AddHeaderCell("#");
                table.AddHeaderCell("Name");
                table.AddHeaderCell("Value");

                table.AddCell("1");
                table.AddCell("Item One");
                table.AddCell("100");

                table.AddCell("2");
                table.AddCell("Item Two");
                table.AddCell("200");

                document.Add(table);

                // Add Footer
                Paragraph footer = new Paragraph("© 2025 Company Name. All Rights Reserved.")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY);
                document.Add(footer);

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}

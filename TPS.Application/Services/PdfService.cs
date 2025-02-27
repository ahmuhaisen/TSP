using iText.Kernel.Pdf;
using iText.Layout.Element;
using System.Reflection.Metadata;
using TPS.Application.Abstractions;
using iText.Layout;
namespace TPS.Application.Services
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> GeneratePdf(string content)
        {
            await Task.Yield();
            using (var memoryStream = new MemoryStream())
            using (var writer = new PdfWriter(memoryStream))
            using (var pdf = new PdfDocument(writer))
            {
                var document = new iText.Layout.Document(pdf);
                document.Add(new Paragraph(content));
                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}

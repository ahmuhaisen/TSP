using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;

namespace TSP.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ApiController
    {
        private readonly IPdfService _pdfService;
        public PdfController(ISender sender,IPdfService pdfService) : base(sender)
        {
            _pdfService = pdfService;
        }
        [HttpPost]
        public async Task<IActionResult> GeneratePdf([FromBody] string content)
        {
            var pdfBytes = await _pdfService.GeneratePdf(content);
            return File(pdfBytes, "application/pdf", "GeneratePdf.pdf");
        }
    }
}

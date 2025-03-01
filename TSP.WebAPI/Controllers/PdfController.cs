using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ApiController
{
    private readonly IPdfService _pdfService;
    public PdfController(ISender sender, IPdfService pdfService) : base(sender)
    {
        _pdfService = pdfService;
    }



    [HttpPost]
    public async Task<IActionResult> GeneratePdf()
    {
        try
        {
            var pdfBytes = await _pdfService.GeneratePdf("");
            return File(pdfBytes, "application/pdf", "Report.pdf");
        }
        catch (Exception ex)
        {
            // Log the exception (e.g., ILogger)
            return StatusCode(500, "Failed to generate PDF: " + ex.Message);
        }
    }
}


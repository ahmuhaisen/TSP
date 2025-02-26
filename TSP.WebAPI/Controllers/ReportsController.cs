using Microsoft.AspNetCore.Mvc;
using System.Data;
using AspNetCore.Reporting;
using MediatR;

namespace TSP.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReportsController(ISender sender,IWebHostEnvironment webHostEnvironment) : base(sender)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Print()
        {
            string extension = "pdf";
            string mimetype = "application/pdf";

            string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "rptEvent.rdlc");
            if (!System.IO.File.Exists(reportPath))
            {
                return NotFound(new { Message = "Report file not found." });
            }
            DataTable data = GetEventsList();
            LocalReport report = new LocalReport(reportPath);
            report.AddDataSource("dsEvent", data);

            var result = report.Execute(RenderType.Pdf, 1, null, mimetype);

            return File(result.MainStream, mimetype, "report." + extension);
        }
        [NonAction]
        public DataTable GetEventsList()
        {
            var data = new DataTable();
            data.Columns.Add("EventId");
            data.Columns.Add("EventName");
            data.Columns.Add("SocietyName");
            DataRow row;
            row = data.NewRow();
            row["EventId"] = 1;
            row["EventName"] = "Junior To Solver";
            row["SocietyName"] = "ACM";

            return data;
        }
    }
}

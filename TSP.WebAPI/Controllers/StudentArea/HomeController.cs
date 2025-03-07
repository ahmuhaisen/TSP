using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.StudentArea.Home.Contracts;
using TPS.Application.Areas.StudentArea.Home.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea
{
    [ApiController]
    [Route($"api/{Constants.APIAreas.Student}/[controller]")]
    public class HomeController : ApiController
    {
        public HomeController(ISender sender):base(sender)
        {
        }
        [HttpGet("recentEvents")]
        [ProducesResponseType(typeof(StudentEventListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecentEvents()
        {
            var query = GetHomeEvents.Query.Create(GetCurrentUserId()!.Value);

            var task = _sender.Send(query);

            return await FromResult(task);
        }

        [HttpGet("homeStatistics")]
        [ProducesResponseType(typeof(StudentHomeStatisticsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HomeStatistics()
        {
            var query = GetHomeStatistics.Query.Create(GetCurrentUserId()!.Value);

            var task = _sender.Send(query);

            return await FromResult(task);
        }
    }
}

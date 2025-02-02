using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TPS.Application.Home.Contracts;
using TPS.Application.Home.Queries;
using TPS.Application.Students.Contracts;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ApiController
    {
        public HomeController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EventListDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Events([FromQuery] Guid advisorId)
        {
            var query = GetHomeEvents.Query.Create(advisorId);
            var task = _sender.Send(query);
            return await FromResult(task);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RecentlyJoinedDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecentlyJoinedMembers([FromQuery] string? searchTerm)
        {
            var query = GetRecentlyJoined.Query.Create(searchTerm);

            var task = _sender.Send(query);

            return await FromResult(task);
        }
        [HttpGet]
        [ProducesResponseType(typeof(HomeStatisticsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HomeStatistics([FromQuery] string? searchTerm)
        {
            var query = GetHomeStatistics.Query.Create(searchTerm);

            var task = _sender.Send(query);

            return await FromResult(task);
        }
    }
}

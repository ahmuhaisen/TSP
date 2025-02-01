using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Events.Contracts;
using TPS.Application.Events.Queries;
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
        public async Task<IActionResult> GetEvents([FromQuery] Guid advisorId)
        {
            var query = GetHomeEvents.Query.Create(advisorId);
            var task = _sender.Send(query);
            return await FromResult(task);
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(List<MembersListDTO>),StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(List<MembersListDTO>),StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult>GetMembers
    }
}

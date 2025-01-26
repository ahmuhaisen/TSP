using MediatR;
using Microsoft.AspNetCore.Mvc;
using TSP.Domain.Shared;
using TPS.Application.Societies.Commands;
using TPS.Application.Societies.Contracts.Requests;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;
using TSP.Domain.Entities;

namespace TPS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvisorController : ApiController
{
    public AdvisorController(ISender sender) : base(sender)
    {}

}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.StudentArea.Socities.Commands;
using TPS.Application.Areas.StudentArea.Students.Commands;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Student}/[controller]")]
public class StudentsController : ApiController
{
    public StudentsController(ISender sender) : base(sender)
    { }



}


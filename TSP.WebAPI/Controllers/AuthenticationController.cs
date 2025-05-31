using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Authentication;
using TPS.Application.Areas.Authentication.Contracts;
using TPS.Infrastructure.Emailing;

namespace TSP.WebAPI.Controllers;

[Route($"api/[controller]")]
[ApiController]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(ISender sender, 
        IAuthenticationService authService) : base(sender)
    {
        _authService = authService;
    }

    // Faculty Member endpoints

    [HttpPost("FacultyMember/Register")]
    public async Task<IActionResult> RegisterFacultyMember([FromBody] FacultyRegisterRequest request)
    {
        var task = _authService.RegisterFacultyMember(request);
        return await FromResult(task);
    }

    [HttpPost("FacultyMember/Login")]
    public async Task<IActionResult> LoginFacultyMember([FromBody] LoginRequest request)
    {
        Console.WriteLine(request.Email);
        Console.WriteLine(request.Password);
        var task = _authService.LoginFacultyMember(request);
        return await FromResult(task);
    }

    // Student endpoints

    [HttpPost("Student/Register")]
    public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterRequest request)
    {
        var task = _authService.RegisterStudent(request);
        return await FromResult(task);
    }

    [HttpPost("Student/Login")]
    public async Task<IActionResult> LoginStudent([FromBody] LoginRequest request)
    {
        var task = _authService.LoginStudent(request);
        return await FromResult(task);
    }

    // Super Admin endpoints

    [HttpPost("SuperAdmin/Login")]
    public async Task<IActionResult> LoginSuperAdmin([FromBody] LoginRequest request)
    {
        var task = _authService.LoginSuperAdmin(request);
        return await FromResult(task);
    }

    // reset password for all users

    [HttpGet("reset")]
    public async Task<IActionResult> ResetUser([FromQuery] string Email, [FromQuery] string url)
    {
        var task = _authService.resetPassword(Email,url);
       
       
        return await FromResult(task);
    }

 
}
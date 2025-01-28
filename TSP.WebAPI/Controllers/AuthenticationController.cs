using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;
using TPS.Application.Common.Contracts.Authentication;
using TSP.Domain.Entities;

namespace TSP.WebAPI.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthenticationController(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    // Faculty Member endpoints

    [HttpPost("FacultyMember/Register")]
    public async Task<IActionResult> RegisterFacultyMember([FromBody] FacultyRegisterRequest request)
    {
        var faculty = new FacultyMember
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            DepartmentId = request.DepartmentId,
            EmployeeNumber = request.EmployeeNumber,
            RankId = request.RankId
        };

        var result = await _userManager.CreateAsync(faculty, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(faculty, "Faculty");
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("FacultyMember/Login")]
    public async Task<IActionResult> LoginFacultyMember([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not FacultyMember || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized("Invalid credentials");

        var token = await _tokenService.GenerateAsync(user);
        return Ok(new { Token = token });
    }

    [HttpPost("FacultyMember/ResetPassword")]
    public async Task<IActionResult> ResetPasswordFacultyMember([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not FacultyMember)
            return BadRequest("Invalid request");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    // Student endpoints

    [HttpPost("Student/Register")]
    public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterRequest request)
    {
        var student = new Student
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            DepartmentId = request.DepartmentId,
            UniversityNumber = request.UniversityNumber
        };

        var result = await _userManager.CreateAsync(student, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(student, "Student");
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("Student/Login")]
    public async Task<IActionResult> LoginStudent([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not Student || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized("Invalid credentials");

        var token = await _tokenService.GenerateAsync(user);
        return Ok(new { Token = token });
    }

    [HttpPost("Student/ResetPassword")]
    public async Task<IActionResult> ResetPasswordStudent([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not Student)
            return BadRequest("Invalid request");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }
}
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Authentication.Contracts;
using TPS.Application.Areas.Shared.Profiles.Contracts.Requests;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Authentication;

public class AuthenticationService(UserManager<ApplicationUser> _userManager,
                                    IEmailService _emailService,
                                   IJwtTokenService _tokenService)
    : IAuthenticationService
{
    public async Task<Result> RegisterFacultyMember(FacultyRegisterRequest request)
    {
        var facultyMember = new FacultyMember
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

        var result = await _userManager.CreateAsync(facultyMember, request.Password);

        if (!result.Succeeded)
            return Result.Failure(Error.CustomError(string.Join(", ", result.Errors)));

        await _userManager.AddToRoleAsync(facultyMember, "Faculty");

        return Result.Success();
    }

    public async Task<Result<LoginResponse>> LoginFacultyMember(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not FacultyMember || !user.IsActive || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Result.Failure<LoginResponse>(Error.InvalidCredentials());

        var token = await _tokenService.GenerateAsync(user);

        var response = new LoginResponse(
            token,
            "FacultyMember",
            user.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email!,
            user.ProfileImageId!
        );

        return Result.Success(response);
    }

    public async Task<Result> RegisterStudent(StudentRegisterRequest request)
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

        if (!result.Succeeded)
            return Result.Failure(Error.CustomError(string.Join(", ", result.Errors)));

        await _userManager.AddToRoleAsync(student, "Student");

        return Result.Success();
    }

    public async Task<Result<LoginResponse>> LoginStudent(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not Student || !user.IsActive || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Result.Failure<LoginResponse>(Error.InvalidCredentials());

        var token = await _tokenService.GenerateAsync(user);

        var response = new LoginResponse(
            token,
            "Student",
            user.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email!,
            user.ProfileImageId!
        );

        return Result.Success(response);
    }

    public async Task<Result<LoginResponse>> LoginSuperAdmin(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Result.Failure<LoginResponse>(Error.InvalidCredentials());

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Contains("SuperAdmin"))
            return Result.Failure<LoginResponse>(Error.InvalidCredentials());

        var token = await _tokenService.GenerateAsync(user, "SuperAdmin");
        var response = new LoginResponse(
            token,
            "SuperAdmin",
            user.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email!,
            user.ProfileImageId!
        );

        return Result.Success(response);
    }
    public async Task<Result<bool>> resetPassword(string Email)
    {
        var user = await _userManager.FindByEmailAsync(Email);
        if (user is null)
            return Result.Failure<bool>(Error.InvalidCredentials());

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var response = new ResetPasswordResponse
        {
            Id = user.Id,
            Token = token
        };

        await _emailService.SendResetLink(user.Email,user.Id,token,user.UserName);
        return Result.Success(true);
    }
 

}

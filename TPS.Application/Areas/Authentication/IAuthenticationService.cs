using TPS.Application.Areas.Authentication.Contracts;
using TPS.Application.Areas.Shared.Profiles.Contracts.Requests;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResponse>> LoginFacultyMember(LoginRequest request);
    Task<Result<LoginResponse>> LoginStudent(LoginRequest request);
    Task<Result> RegisterFacultyMember(FacultyRegisterRequest request);
    Task<Result> RegisterStudent(StudentRegisterRequest request);
    Task<Result<LoginResponse>> LoginSuperAdmin(LoginRequest request);
    Task<Result<ResetPasswordResponse>> resetPassword(string Email);
}

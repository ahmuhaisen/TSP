using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Users.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Users
{
    public class UserService(ApplicationDbContext context) : IUserService
    {
        public async Task<Result<List<UserDTO>>> GetAllUsers()
        {
            var facultyData = await GetFacultyMembersAsync();
            var studentData = await GetStudentsAsync();

            var allUsers = facultyData.Concat(studentData).ToList();
            return Result.Success(allUsers);
        }

        public async Task<Result<List<UserDTO>>> GetAllFacultyMembers()
        {
            var facultyData = await GetFacultyMembersAsync();
            return Result.Success(facultyData);
        }

        public async Task<Result<List<UserDTO>>> GetAllStudents()
        {
            var studentData = await GetStudentsAsync();
            return Result.Success(studentData);
        }

        private async Task<List<UserDTO>> GetFacultyMembersAsync()
        {
            return await context.FacultyMembers
                .Select(x => new UserDTO
                {
                    id = x.Id,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    UserType=UserType.FacultyMember
                })
                .ToListAsync();
        }

        private async Task<List<UserDTO>> GetStudentsAsync()
        {
            return await context.Students
                .Select(x => new UserDTO
                {
                    id = x.Id,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    UserType=UserType.Student
                })
                .ToListAsync();
        }
    }
}

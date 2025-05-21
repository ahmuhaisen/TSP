using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Areas.Shared.Users.Contracts;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Abstractions
{
    public interface IUserService
    {
        Task<Result<List<UserDTO>>> GetAllUsers();
        Task<Result<List<UserDTO>>> GetAllFacultyMembers();
        Task<Result<List<UserDTO>>> GetAllStudents();
    }
}

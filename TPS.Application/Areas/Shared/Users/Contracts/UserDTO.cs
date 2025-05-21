using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Enums;

namespace TPS.Application.Areas.Shared.Users.Contracts
{
    public record UserDTO
    {
        public Guid id {  get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public UserType UserType { get; set; }
    }
}

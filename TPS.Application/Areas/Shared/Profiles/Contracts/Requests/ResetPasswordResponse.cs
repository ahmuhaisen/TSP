using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.Shared.Profiles.Contracts.Requests;
public class ResetPasswordResponse
{
    public required Guid Id { get; set; }
    public required string Token { get; set; }

}
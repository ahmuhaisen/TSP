using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.AdminArea.Students.Contracts.Requests;

public class EditMemberRequest
{
    public Guid StudentId { get; set; }
    public Guid SocietyId { get; set; }
    public required string Position { get; set; }
}

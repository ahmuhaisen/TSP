using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Students.Contracts.Requests;

public class AddCommitteeRequest
{
    public Guid StudentId { get; set; }
    public Guid SocietyId { get; set; }
    public required string Position {  get; set; }
    public required DateOnly StartDate { get; set; }
}

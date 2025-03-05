using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Students.Contracts.Requests;

public class AddCommitteeRequest
{
    public required string Position {  get; set; }
    public required DateOnly StartDate { get; set; }
}

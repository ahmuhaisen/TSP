using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Events.Contracts;

public class AddEventRequest
{
    public Guid SocietyId { get; set; }
    public Guid CommitteeId { get; set; }   
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public required string Type { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public bool IsAttendanceFormEnabled { get; set; }

}

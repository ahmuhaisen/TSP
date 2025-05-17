using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Events.Contracts;

public class EventSimpleDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string SocietyName { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; } = null!;
    public string Location { get; set; } = null!;
    public bool IsAttendeesFormEnabled { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.Shared.Events.Contracts;

public class EventBasicDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}

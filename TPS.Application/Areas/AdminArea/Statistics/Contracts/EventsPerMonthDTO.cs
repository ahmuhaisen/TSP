using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.AdminArea.Statistics.Contracts;

public class EventsPerMonthDTO
{
    public required string Date { get; set; } 
    public int Events {  get; set; }
}

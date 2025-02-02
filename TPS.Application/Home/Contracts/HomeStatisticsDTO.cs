using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Home.Contracts
{
    public class HomeStatisticsDTO
    {
        public required int TotalMembers { get; set; }
        public required int TotalSocieties { get; set; }
        public required int TotalCompletedEvents { get; set; }
        public int? TotalAttendees { get; set; }
    }
}

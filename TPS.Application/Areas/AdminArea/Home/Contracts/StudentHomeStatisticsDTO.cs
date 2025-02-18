using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.AdminArea.Home.Contracts
{
    public class StudentHomeStatisticsDTO
    {
        public required int NoSocieties { get; set; }
        public required int NoAttendedEvents { get; set; }
    }
}

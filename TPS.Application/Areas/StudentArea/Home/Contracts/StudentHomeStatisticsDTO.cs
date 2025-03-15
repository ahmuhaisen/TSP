using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Home.Contracts
{
    public class StudentHomeStatisticsDTO
    {
        public required int NumSocieties { get; set; }
        public required int NumAttendedEvents { get; set; }
    }
}

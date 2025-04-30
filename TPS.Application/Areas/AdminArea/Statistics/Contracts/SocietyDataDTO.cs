using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.AdminArea.Statistics.Contracts;

public class SocietyDataDTO
{
    public Guid id { get; set; }
    public required string SocietyName { get; set; }
    public int Members {  get; set; }
    public int Events { get; set; }
}

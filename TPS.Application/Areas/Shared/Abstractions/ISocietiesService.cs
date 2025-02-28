using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Abstractions;

public interface ISocietiesService
{
    Task<Result<SocietyDTO>> getSocietyById(Guid SocietyId);
}

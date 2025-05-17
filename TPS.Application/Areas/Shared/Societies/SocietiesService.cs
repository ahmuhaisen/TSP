using Microsoft.EntityFrameworkCore;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Societies;

public class SocietiesService(ApplicationDbContext context): ISocietiesService
{
   
    public async Task<Result<SocietyDTO>> getSocietyById(Guid SocietyId)
    {
        if (!context.Societies.Any(s => s.Id == SocietyId))
            return Result.Failure<SocietyDTO>(Error.NotFound(nameof(Society), SocietyId.ToString()));

        var data = await context.Societies
            .Include(s => s.SocietiesMembers)
            .Include(s => s.Advisor)
            .AsNoTracking()
            .Where(s => s.Id == SocietyId)
            .Select(s => new SocietyDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                LogoId = s.LogoId,
                ThemeColor = s.ThemeColor,
                CreationDate = s.CreationDate,
                NumberOfMembers = s.SocietiesMembers.Count(),
                Advisor = new FacultyMemberBasicDTO
                {
                    Id = s.Advisor.Id,
                    FullName = $"{s.Advisor.FirstName} {s.Advisor.LastName}",
                    LogoId = s.Advisor.ProfileImageId??""
                }
            }).FirstAsync();

        return Result.Success(data);
    }

    public async Task<Result<List<SocietyDTO>>> getAllSocieties()
    {
        var data = await context.Societies
            .Include(s => s.SocietiesMembers)
            .Include(s => s.Advisor)
            .AsNoTracking()
            .Select(s => new SocietyDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                LogoId = s.LogoId,
                ThemeColor = s.ThemeColor,
                CreationDate = s.CreationDate,
                NumberOfMembers = s.SocietiesMembers.Count(),
                Advisor = new FacultyMemberBasicDTO
                {
                    Id = s.Advisor.Id,
                    FullName = $"{s.Advisor.FirstName} {s.Advisor.LastName}",
                    LogoId = s.Advisor.ProfileImageId ?? ""
                }
            }).ToListAsync();
        return Result.Success(data);
    }
}

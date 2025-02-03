using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Societies.Queries;

public class GetAdvisorSocieties
{
    public sealed class Query : IQuery<Result<List<SocietyListDTO>>>
    {
        public Guid AdvisorId { get; set; }

        private Query(Guid id)
        {
            AdvisorId = id;
        }

        public static Query Create(Guid AdvisorId) => new Query(AdvisorId);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<SocietyListDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<SocietyListDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var member = await _context.FacultyMembers.FirstOrDefaultAsync(s => s.Id == request.AdvisorId);

            if (member is null)
            {
                return Result.Failure<List<SocietyListDTO>>(Error.NotFound(nameof(FacultyMember), request.AdvisorId.ToString()));
            }

            var allSocietiesQuery = _context.Societies;

            var facultyMemberSocieties = await allSocietiesQuery
                .AsNoTracking()
                .Where(s => s.AdvisorId == request.AdvisorId)
                .Select(s => new SocietyListDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    LogoId = s.LogoId,
                    ThemeColor = s.ThemeColor,
                    CreationDate = s.CreationDate,

                })
                .ToListAsync();

            return Result.Success(facultyMemberSocieties);
        }
    }
}

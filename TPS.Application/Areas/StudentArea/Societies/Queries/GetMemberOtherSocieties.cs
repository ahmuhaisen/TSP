using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Societies.Queries;

public class GetMemberOtherSocieties
{
    public sealed class Query : IQuery<Result<List<SocietyListDTO>>>
    {
        public Guid MemberId { get; set; }

        private Query(Guid id)
        {
            MemberId = id;
        }

        public static Query Create(Guid MemberId) => new Query(MemberId);
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
            var member = await _context.Students.FirstOrDefaultAsync(s => s.Id == request.MemberId);

            if (member is null)
            {
                return Result.Failure<List<SocietyListDTO>>(Error.NotFound(nameof(Student), request.MemberId.ToString()));
            }
            var allSocieties = await _context.SocietiesMembers
                .AsNoTracking()
                .Where(s => s.StudentId == request.MemberId)
                .Select(s =>s.SocietyId)
                .ToListAsync();

            var otherSocieties = await _context.Societies
                .AsNoTracking()
                .Where(s=>!allSocieties.Contains(s.Id))
                .Select(s => new SocietyListDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    LogoId = s.LogoId,
                    CreationDate = s.CreationDate,
                })
                .ToListAsync();


            return Result.Success(otherSocieties);
        }
    }
}

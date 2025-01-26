
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Societies.Contracts;
using TPS.Application.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
public class GetCommitteeMembers
{
    
    public sealed class Query : IQuery<Result<List<MembersListDTO>>>
    {
        public Guid SocitieyId { get; set; }

        private Query(Guid Id)
        {
            SocitieyId = Id;
        }

        public static Query Create(Guid SocitieyId) => new Query(SocitieyId);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<MembersListDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<MembersListDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var society = await _context.Societies.FirstOrDefaultAsync(s =>s.Id ==request.SocitieyId);
            if (society is null)
            {
                return Result.Failure<List<MembersListDTO>>(Error.NotFound(nameof(Society), request.SocitieyId.ToString()));

            }
            var societiesMembers =  _context.SocietiesMembers.AsQueryable();
            var data = await societiesMembers
                .Include(s => s.Society)
                .Include(s=> s.Student)
                .AsNoTracking()
                .Where(s=>s.SocietyId == request.SocitieyId&& s.IsActive&&s.IsCommittee)
                .Select(
                 s=>new MembersListDTO
                 {
                     FirstName = s.Student.FirstName,
                     LastName = s.Student.LastName,
                     Position = s.Position,
                     JoinDate = s.JoinDate,
                 }
                )
                .ToListAsync();

            return Result.Success(data);
        }
    }
}

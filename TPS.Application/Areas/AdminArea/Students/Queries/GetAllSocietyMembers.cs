
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
public class GetAllSocietyMembers
{

    public sealed class Query : IQuery<Result<List<MembersListDTO>>>
    {
        public Guid SocitieyId { get; set; }
        public bool IsCommittee { get; set; }

        private Query(Guid Id, bool isCommittee)
        {
            SocitieyId = Id;
            IsCommittee = isCommittee;
        }

        public static Query Create(Guid SocitieyId, bool isCommittee) => new Query(SocitieyId, isCommittee);
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
            var society = await _context.Societies.FirstOrDefaultAsync(s => s.Id == request.SocitieyId);
            if (society is null)
            {
                return Result.Failure<List<MembersListDTO>>(Error.NotFound(nameof(Society), request.SocitieyId.ToString()));

            }
            var societiesMembers = _context.SocietiesMembers.AsQueryable();
            var data = await societiesMembers
                .AsNoTracking()
                .Where(
                s => s.SocietyId == request.SocitieyId &&
                s.IsCommittee == request.IsCommittee)
                .Select(
                 s => new MembersListDTO
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


using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.Shared.Abstractions;
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

    public sealed class Handler(IStudentsService studentsService) : IQueryHandler<Query, Result<List<MembersListDTO>>>
    {
        public async Task<Result<List<MembersListDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await studentsService.
                        getSocietyMembers(request.SocitieyId, request.IsCommittee);

            
        }
    }
}

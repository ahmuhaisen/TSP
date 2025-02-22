using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Societies.Queries;

public class GetSocietyById
{
    public sealed class Query : IQuery<Result<Contracts.SocietyDTO>>
    {
        public Guid Id { get; set; }

        private Query(Guid id)
        {
            Id = id;
        }

        public static Query Create(Guid id) => new Query(id);
    }

    public sealed class Handler(ISocietiesService societiesService) : IQueryHandler<Query, Result<Contracts.SocietyDTO>>
    {
        public async Task<Result<Contracts.SocietyDTO>> Handle(Query request, CancellationToken cancellationToken)
        {

            return await societiesService.getSocietyById(request.Id);  
        }
    }
}

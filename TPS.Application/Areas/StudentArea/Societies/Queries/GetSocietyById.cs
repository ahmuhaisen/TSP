
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.Shared.Abstractions;
using TSP.Domain.Shared;


public class GetSocietyById
{
    public sealed class Query : IQuery<Result<SocietyDTO>>
    {
        public Guid Id { get; set; }
        private Query(Guid id)
        {
            Id = id;
        }

        public static Query Create(Guid id) => new Query(id);
    }

    public sealed class Handler(ISocietiesService societiesService) : IQueryHandler<Query, Result<SocietyDTO>>
    {
        public async Task<Result<SocietyDTO>> Handle(Query request, CancellationToken cancellationToken)
        {

            return await societiesService.getSocietyById(request.Id);
        }
    }
}

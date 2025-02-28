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

            if (!_context.Societies.Any(s => s.Id == request.Id))
                return Result.Failure<Contracts.SocietyDTO>(Error.NotFound(nameof(Society), request.Id.ToString()));

            var data = await _context.Societies
                .Include(s => s.SocietiesMembers)
                .Include(s => s.Advisor)
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
                .Select(s => new Contracts.SocietyDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    LogoId = s.LogoId,
                    ThemeColor = s.ThemeColor,
                    CreationDate = s.CreationDate,
                    NumberOfMembers = s.SocietiesMembers.Count(),
                    Advisor = new Contracts.FacultyMemberBasicDTO
                    {
                        Id = s.Advisor.Id,
                        FullName = $"{s.Advisor.FirstName} {s.Advisor.LastName}",
                        LogoId = s.LogoId
                    }
                })
                .SingleAsync(cancellationToken);

            return await societiesService.getSocietyById(request.Id);  
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Societies.Queries;

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

    public sealed class Handler : IQueryHandler<Query, Result<SocietyDTO>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<SocietyDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!_context.Societies.Any(s => s.Id == request.Id))
                return Result.Failure<SocietyDTO>(Error.NotFound(nameof(Society), request.Id.ToString()));
            
            var data = await _context.Societies
                .Include(s => s.SocietiesMembers)
                .Include(s => s.Advisor)
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
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
                    }
                })
                .SingleAsync(cancellationToken);
            
            return Result.Success(data);
        }
    }
}

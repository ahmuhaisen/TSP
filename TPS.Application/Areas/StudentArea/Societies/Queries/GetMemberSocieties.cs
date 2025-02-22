using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Societies.Queries;

public class GetMemberSocieties
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


            var memberSocieties = await _context.SocietiesMembers
                .AsNoTracking()
                .Include(s => s.Society)
                .Where(s => s.StudentId == request.MemberId)
                .Select(s => new SocietyListDTO
                {
                 Id = s.SocietyId,
                 Name = s.Society.Name,
                 LogoId = s.Society.LogoId,
                 CreationDate = s.Society.CreationDate,

                })
                .ToListAsync();

            return Result.Success(memberSocieties);
        }
    }
}

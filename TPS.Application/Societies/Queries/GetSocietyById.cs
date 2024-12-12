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
            //var dataDao = await _context.Societies.FirstOrDefaultAsync(s => s.ID == request.Id);
            //if (dataDao == null)
            //{
            //    return Result.Failure<SocietyDTO>(
            //        Error.NotFound(nameof(Society),
            //        request.Id.ToString())
            //    );
            //}
            //// i think we need a mapper hehe
            //var dataDto = new SocietyDTO
            //{
            //    Name = dataDao.Name,
            //    Description = dataDao.Description,
            //    LogoId = dataDao.LogoID,
            //    CreationDate = dataDao.CreationDate,
            //    ThemeColor = dataDao.ThemeColor
                
                
            //};
            //return Result.Success(dataDto);
            throw new NotImplementedException();
        }
    }
}

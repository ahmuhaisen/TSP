//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TPS.Application.Abstractions.Messaging;
//using TPS.Application.Areas.AdminArea.Events.Contracts;
//using TPS.Application.Areas.Shared.Schools.Contracts;
//using TPS.Infrastructure.Data;
//using TSP.Domain.Entities;
//using TSP.Domain.Shared;

//namespace TPS.Application.Areas.StudentArea.Events.Queries;

//public class GetStudentEvents
//{
//    public sealed class Query : IQuery<Result<List<EventsDTO>>> {
//        public Guid StudentId { get; set; }
//        public static Query Create(Guid id)
//        {
//            return new Query
//            {
//                StudentId = id
//            };
//        }
    
    
//    }

//    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<EventsDTO>>>
//    {
//        public async Task<Result<List<EventsDTO>>> Handle(Query request, CancellationToken cancellationToken)
//        {
//            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == request.StudentId);
//            if (student == null)
//            {
//                return Result.Failure<List<EventsDTO>>(Error.NotFound(nameof(Student), request.StudentId.ToString()));
//            }
//            /*
//             * 
//             *     public required Guid Id { get; set; }
//        public required string EventName { get; set; }
//        public DateTime DateTime { get; set; }
//        public string? LocationString { get; set; }
//        public required string ApprovalStatus { get; set; }
//        public required string Description { get; set; }
//        public required string SocietyName { get; set; }
//             */

//            var data = _context.Events
//                .AsNoTracking()
//                .Include(s=>s.Society)
//                .Where(s => s.SocietyId == request.StudentId)
//                .Select(s => new EventsDTO
//                { 
//                    Id = s.Id,
//                    EventName = s.Name,
//                    DateTime = s.StartTime,
//                    LocationString = s.LocationString,
//                    Description = s.Description,
//                    SocietyName = s.Society.Name,
//                    ApprovalStatus = 
//                });
        
        
//        }
//    }
//}

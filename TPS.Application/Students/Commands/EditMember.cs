//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TPS.Application.Abstractions.Messaging;
//using TPS.Infrastructure.Data;
//using TSP.Domain.Entities;
//using TSP.Domain.Shared;

//namespace TPS.Application.Students.Commands;

//public class EditMember
//{
//    public sealed class Command : ICommand<Result<Guid>>
//    {
//        public Guid StudentId { get; set; }
//        public Guid SocietyId { get; set; }
//        public static Command Create(Guid id, Guid SocietyId)
//        {
//            return new Command
//            {
//                StudentId = id,
//                SocietyId = SocietyId
//            };
//        }
//    }

//    public sealed class Handler : ICommandHandler<Command, Result<Guid>>
//    {
//        private ApplicationDbContext _context { get; }

//        public Handler(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
//        {
            
//        }
//    }
//}

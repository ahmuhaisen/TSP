
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Socities.Commands;

public class LeaveSociety
{
    public sealed class Command : ICommand<Result>
    {
        public Guid StudentId { get; set; }
        public Guid SocietyId { get; set; }
        public static Command Create(Guid StudentId,Guid SocietyId)
        {
            return new Command
            {
                StudentId = StudentId,
                SocietyId = SocietyId
            };
        }
    }
    public sealed class Handler(IStudentsService studentsService) : ICommandHandler<Command, Result>
    {

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await studentsService.deleteMember(request.StudentId, request.SocietyId);
        }
    }



}

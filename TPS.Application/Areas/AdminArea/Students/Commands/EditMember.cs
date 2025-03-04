using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Students.Commands;

public class EditMember
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public Guid StudentId { get; set; }
        public Guid SocietyId { get; set; }
        public required string Position { get; set; }
        public static Command Create(Guid studentId,Guid societyId,string position)
        {
            return new Command
            {
               StudentId = studentId,
               SocietyId = societyId,
               Position = position
            };
        }
    }


    public sealed class Handler(IStudentsService studentsService) : ICommandHandler<Command, Result<Guid>>
    {
     
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await studentsService.editMember(request.StudentId,request.SocietyId,request.Position);
        }
    }
}

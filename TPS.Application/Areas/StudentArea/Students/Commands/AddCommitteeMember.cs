using System.Runtime.CompilerServices;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Students.Commands;

public class AddCommitteeMember
{
    public sealed class Command : ICommand<Result<Guid>>
    {

        public Guid StudentId { get; set; }
        public Guid SocietyId { get; set; }
        public required AddCommitteeRequest committeRequest { get; set; }
        public static Command Create(Guid studentId, Guid societyId, AddCommitteeRequest committeRequest)
        {
            return new Command
            {
                StudentId = studentId,
                SocietyId = societyId,
                committeRequest = committeRequest,
            };
        }
    }

    public sealed class Handler(IStudentsService studentsService) : ICommandHandler<Command, Result<Guid>>
    {

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await studentsService.addCommitte(request.StudentId,request.SocietyId,request.committeRequest);
        }
    }

}
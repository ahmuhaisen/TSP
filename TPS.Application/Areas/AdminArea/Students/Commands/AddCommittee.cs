

using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Students.Commands;

public class AddCommittee
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public required AddCommitteeRequest committeRequest { get; set; }
        public static Command Create(AddCommitteeRequest committeRequest)
        {
            return new Command
            {
                committeRequest = committeRequest,
            };
        }
    }

    public sealed class Handler(IStudentsService studentsService) : ICommandHandler<Command, Result<Guid>>
    {

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await studentsService.addCommitte(request.committeRequest);
        }
    }
}

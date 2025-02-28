using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Students.Contracts.Requests;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Students.Commands;

public class EditMember
{
    public sealed class Command : ICommand<Result<Guid>>
    {
       public required EditMemberRequest editRequest {  get; set; }
        public static Command Create(EditMemberRequest editRequest)
        {
            return new Command
            {
               editRequest = editRequest
            };
        }
    }


    public sealed class Handler(IStudentsService studentsService) : ICommandHandler<Command, Result<Guid>>
    {
     
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await studentsService.editMember(request.editRequest);
        }
    }
}

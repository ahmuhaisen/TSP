
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.StudentArea.Events.Contracts;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Events.Commands;

public class AddEvent
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public required AddEventRequest eventRequest { get; set; }

        public static Command Create(AddEventRequest eventRequest)
        {
            return new Command
            {
                eventRequest = eventRequest,
            };
        }
    }
    public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var Request = request.eventRequest;
            var member = await context.SocietiesMembers
            .FirstOrDefaultAsync(s => s.StudentId == Request.CommitteeId && s.SocietyId == Request.SocietyId);
            
            if (member is null)
            {
                return Result.Failure<Guid>(Error.NotFound(nameof(Student), Request.CommitteeId.ToString()));
            }
            if (!member.IsCommittee)
            {
                return Result.Failure<Guid>(Error.CustomError("the member is not a committee"));
            }

            var tempEvent = new Event
            {
                Id = Guid.NewGuid(),
                StudentId = Request.CommitteeId,
                SocietyId = Request.SocietyId,
                IsAttendeesFormEnabled = Request.IsAttendanceFormEnabled,
                StartTime = Request.StartDate,
                EndTime = Request.EndDate,
                Type = Request.Type,
                LocationString = Request.Location,
                RequestTime = DateTime.Now
            };

            await context.Events.AddAsync(
              tempEvent
             );
            var checkChanges = context.SaveChanges();
            if (checkChanges <= 0)
            {
                return Result.Failure<Guid>(Error.InternalServerError("could not save record"));
            }


            return Result.Success(tempEvent.Id);
        }
    }
}


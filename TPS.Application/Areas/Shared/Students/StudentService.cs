using Microsoft.EntityFrameworkCore;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Students;

public class StudentService(ApplicationDbContext context) : IStudentsService
{
    public async Task<Result<Guid>> addCommitte(Guid StudentId, Guid SocietyId, AddCommitteeRequest request)
    {
        var data = await context.SocietiesMembers
           .FirstOrDefaultAsync(
           s => s.StudentId == StudentId &&
           s.SocietyId == SocietyId);

        if (data is null)
        {
            return Result.Failure<Guid>(Error.GuidInvalid(StudentId));

        }
        if (data.IsCommittee == true)
        {
            return Result.Failure<Guid>(Error.ValueInvalid(nameof(Student), StudentId.ToString()));
        }
        data.IsCommittee = true;
        data.Position = request.Position;
        data.JoinDate = request.StartDate;
        var check = context.SaveChanges();
        if (check > 0)
        {
            return Result.Success(StudentId);
        }
        else
        {
            return Result.Failure<Guid>(Error.ValueInvalid(nameof(Student), StudentId.ToString()));
        }
    }

    public async Task<Result> deleteMember(Guid memberId, Guid SocietyId)
    {
        var data = await context.SocietiesMembers
                                .FirstOrDefaultAsync(s => s.StudentId == memberId && s.SocietyId == SocietyId);
        if (data is null)
        {
            return Result.Failure(Error.NotFound(nameof(SocietiesMembers)));
        }
        if (data.IsCommittee)
        {
            return Result.Failure(Error.ValueInvalid("The user is committee member"));
        }

        context.SocietiesMembers.Remove(data);
        var changes = await context.SaveChangesAsync();
        if (changes <= 0)
        {
            return Result.Failure(Error.InternalServerError("Something wrong happend in the process"));
        }

        return Result.Success();
    }

    public async Task<Result<Guid>> editMember(Guid StudentId, Guid SocietyId, string Position)
    {
        if (await context.Societies.FirstOrDefaultAsync(s => s.Id == SocietyId) is null)
        {
            return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society), StudentId.ToString()));

        }
        var data = await context.SocietiesMembers
            .Include(x => x.Society)
            .FirstOrDefaultAsync(s => s.StudentId == StudentId
            && s.SocietyId == SocietyId);

        if (data is null)
        {
            return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society), StudentId.ToString()));
        }

        data.Position = Position;
        var check = await context.SaveChangesAsync();
        if (check <= 0)
        {
            return Result.Failure<Guid>(Error.InternalServerError(StudentId.ToString()));
        }
        return Result.Success(data.StudentId);
    }
    public async Task<Result<List<MembersListDTO>>> getSocietyMembers(Guid SocietyId, bool IsCommittee)
    {
        var check = await context.Societies.FirstOrDefaultAsync(s => s.Id == SocietyId);
        if (check == null)
        {
            return Result.Failure<List<MembersListDTO>>(Error.NotFound(nameof(Society)));
        }

        var data = await context.SocietiesMembers
            .AsNoTracking()
            .Include(s => s.Student)
            .Where(s => s.SocietyId == SocietyId && s.IsCommittee == IsCommittee)
            .Select(s => new MembersListDTO
            {
                Id = s.Student.Id,
                FirstName = s.Student.FirstName,
                LastName = s.Student.LastName,
                Position = s.Position,
                JoinDate = s.JoinDate,
            }).ToListAsync();

        return Result.Success(data);
    }
}

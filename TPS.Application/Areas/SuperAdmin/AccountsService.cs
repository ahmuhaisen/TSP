using Microsoft.EntityFrameworkCore;
using TPS.Application.Areas.SuperAdmin.Contracts;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.SuperAdmin;


public class AccountsService(ApplicationDbContext _context, IEmailService _emailService) : IAccountsService
{
    public async Task<Result<List<PendingAccountBasicDto>>> GetPendingAccountsAsync()
    {
        var pendingStudentAccounts = await _context.Students
            .AsNoTracking()
            .Include(x => x.Department)
            .Where(x => !x.IsActive)
            .Select(x => new PendingAccountBasicDto
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                Email = x.Email!,
                UserType = "Student",
                DepartmentName = x.Department!.Name,
                RegisteredAt = x.RegisteredAt
            }).ToListAsync();

        var pendingFacultyMemberAccounts = await _context.FacultyMembers
            .AsNoTracking()
            .Include(x => x.Rank)
            .Include(x => x.Department)
            .Where(x => !x.IsActive)
            .Select(x => new PendingAccountBasicDto
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                Email = x.Email!,
                UserType = "Faculty Member",
                Rank = x.Rank.Title,
                DepartmentName = x.Department!.Name,
                RegisteredAt = x.RegisteredAt
            }).ToListAsync();

        var pendingAccounts = new List<PendingAccountBasicDto>();
        pendingAccounts.AddRange(pendingStudentAccounts);
        pendingAccounts.AddRange(pendingFacultyMemberAccounts);

        return Result.Success(pendingAccounts);
    }

    public async Task<Result> ApproveAccountAsync(Guid id, UserType userType)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);

        if (user is null)
            return Result.Failure(Error.NotFound(nameof(ApplicationUser)));

        user.IsActive = true;

        _context.Users.Update(user);

        await _context.SaveChangesAsync();

        await _emailService.SendWelcomingEmail(user.Email!, $"{user.FirstName} {user.LastName}", userType);

        return Result.Success();
    }

    public async Task<Result> RejectAccountAsync(Guid id, UserType userType)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);

        if (user is null)
            return Result.Failure(Error.NotFound(nameof(ApplicationUser)));

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Profiles.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Profiles.Command
{
    public sealed class UpdateProfile
    {
        public sealed class Command : ICommand<Result<Guid>>
        {
            public Guid Id { get; set; }
            public string? ProfileImageId { get; set; }
            public string? FullName { get; set; }
            public string? Email { get; set; }
            public string? Number { get; set; }
            public string? UserType { get; set; }
            public static Command Create(Guid id,
                                        string profileImageId,
                                        string fullName,
                                        string email,
                                        string number,
                                        string userType)
            {
                return new Command
                {
                    Id = id,
                    ProfileImageId = profileImageId,
                    FullName = fullName,
                    Email = email,
                    Number = number,
                    UserType = userType
                };
            }
        }
        public sealed class Handler(ApplicationDbContext context, IGitHubService _FileManager) : ICommandHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.UserType == "Student")
                {
                    var student = context.Students
                                          .AsNoTracking()
                                          .FirstOrDefault(s => s.Id == request.Id);

                    if (student is null)
                        return Result.Failure<Guid>(Error.NotFound(request.Id.ToString()));
                    if (!string.IsNullOrWhiteSpace(request.FullName))
                    {
                        var names = request.FullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                        student.FirstName = names.ElementAtOrDefault(0);
                        student.LastName = names.ElementAtOrDefault(1);
                    }
                    if (request.Email != null)
                    {
                        student.Email = request.Email;
                    }
                    if (request.Number != null)
                    {
                        student.UniversityNumber = request.Number;
                    }
                    if (request.ProfileImageId != null)
                    {
                        student.ProfileImageId = request.ProfileImageId;
                    }
                    context.Students.Update(student);
                }

                if (request.UserType == "Faculty")
                {
                    var facultyMember = context.FacultyMembers
                                          .AsNoTracking()
                                          .FirstOrDefault(s => s.Id == request.Id);

                    if (facultyMember is null)
                        return Result.Failure<Guid>(Error.NotFound(request.Id.ToString()));
                    if (!string.IsNullOrWhiteSpace(request.FullName))
                    {
                        var names = request.FullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                        facultyMember.FirstName = names.ElementAtOrDefault(0);
                        facultyMember.LastName = names.ElementAtOrDefault(1);
                    }
                    if (request.Email != null)
                    {
                        facultyMember.Email = request.Email;
                    }
                    if (request.Number != null)
                    {
                        facultyMember.EmployeeNumber = request.Number;
                    }
                    if (request.ProfileImageId != null)
                    {
                        facultyMember.ProfileImageId = request.ProfileImageId;
                    }
                    context.FacultyMembers.Update(facultyMember);
                }
                //if (!string.IsNullOrWhiteSpace(request.ProfileImageId))
                //{
                //    var result = await _FileManager.updateFile(nameof(ApplicationUser), request.ProfileImageId);
                //    if (result.IsFailure)
                //    {
                //        return Result.Failure<Guid>(Error.ValueInvalid(result.Error.Message));
                //    }

                //    string profileImageId = ResponseEnvelope.Success(result.Data!).ResponseData?.ToString() ?? "";

                //}
                await context.SaveChangesAsync();
                return Result.Success(request.Id);
            }
        }
    }
}

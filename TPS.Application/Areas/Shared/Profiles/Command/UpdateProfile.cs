using Azure.Identity;
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
using static TPS.Application.Areas.Shared.Profiles.Command.UpdateProfile;

namespace TPS.Application.Areas.Shared.Profiles.Command
{
    public sealed class UpdateProfile
    {
        public sealed class Command : ICommand<Result<Guid>>
        {
            public Guid Id { get; set; }
            public string? ProfileImageId { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Number { get; set; }
            public required string UserType { get; set; }
            public static Command Create(Guid id,
                                        string firstName,
                                        string lastName,
                                        string profileImageId,
                                        string email,
                                        string number,
                                        string userType)
            {
                return new Command
                {
                    Id = id,
                    FirstName = firstName,
                    LastName= lastName,
                    ProfileImageId = profileImageId,
                    Email = email,
                    Number = number,
                    UserType = userType
                };
            }
        }

        public sealed class Handler(ApplicationDbContext context, IGitHubService FileManager) : ICommandHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var strategy = ProfileUpdateStrategyFactory.Create(request.UserType, context, FileManager);
                return await strategy.UpdateAsync(request);
            }
        }

        public interface IProfileUpdateStrategy
        {
            Task<Result<Guid>> UpdateAsync(Command request);
        }

        public abstract class BaseProfileUpdateStrategy : IProfileUpdateStrategy
        {
            protected readonly ApplicationDbContext _context;
            protected readonly IGitHubService _FileManager;
            protected BaseProfileUpdateStrategy(ApplicationDbContext context, IGitHubService FileManager)
            {
                _context = context;
                _FileManager = FileManager;
            }
            protected async Task<Result<string>> UploadProfileImageAsync(string profileImageId)
            {
                var result = await _FileManager.uploadFile(nameof(ApplicationUser), profileImageId);
                if (result.IsFailure)
                {
                    return Result.Failure<string>(Error.ValueInvalid(result.Error.Message));
                }
                string uploadId = ResponseEnvelope.Success(result.Data!).ResponseData?.ToString() ?? "";
                return Result.Success(uploadId);
            }
            public abstract Task<Result<Guid>> UpdateAsync(Command request);
        }

        public class StudentUpdateStrategy : BaseProfileUpdateStrategy
        {
            public StudentUpdateStrategy(ApplicationDbContext context, IGitHubService FileManager) : base(context, FileManager)
            {
            }
            public override async Task<Result<Guid>> UpdateAsync(Command request)
            {
                var student = _context.Students
                              .AsNoTracking()
                              .FirstOrDefault(s => s.Id == request.Id);

                if (student is null)
                    return Result.Failure<Guid>(Error.NotFound(request.Id.ToString()));
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                {
                    student.FirstName = request.FirstName;   
                }
                if (!string.IsNullOrWhiteSpace(request.LastName))
                {
                    student.LastName = request.LastName;   
                }
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    student.Email = request.Email;
                }
                if (!string.IsNullOrWhiteSpace(request.Number))
                {
                    student.UniversityNumber = request.Number;
                }
                // TODO: Test image update
                if (!string.IsNullOrWhiteSpace(request.ProfileImageId))
                {
                    var result = await UploadProfileImageAsync(request.ProfileImageId);
                    if (result.IsFailure)
                    {
                        return Result.Failure<Guid>(Error.ValueInvalid(result.Error.Message));
                    }
                    student.ProfileImageId = result.Data;
                }
                else
                {
                    student.ProfileImageId= null;
                }
                    _context.Students.Update(student);
                await _context.SaveChangesAsync();
                return Result.Success(student.Id);
            }
        }

        public class FacultyUpdateStrategy : BaseProfileUpdateStrategy
        {
            public FacultyUpdateStrategy(ApplicationDbContext context, IGitHubService FileManager) : base(context, FileManager)
            {
            }
            public override async Task<Result<Guid>> UpdateAsync(Command request)
            {
                var facultyMember = _context.FacultyMembers
                                    .AsNoTracking()
                                    .FirstOrDefault(s => s.Id == request.Id);

                if (facultyMember is null)
                    return Result.Failure<Guid>(Error.NotFound(request.Id.ToString()));
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                {
                    facultyMember.FirstName = request.FirstName;
                }
                if (!string.IsNullOrWhiteSpace(request.LastName))
                {
                    facultyMember.LastName = request.LastName;
                }
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    facultyMember.Email = request.Email;
                }
                if (!string.IsNullOrWhiteSpace(request.Number))
                {
                    facultyMember.EmployeeNumber = request.Number;
                }
                // TODO: Test image Update
                if (!string.IsNullOrWhiteSpace(request.ProfileImageId))
                {
                    var result = await UploadProfileImageAsync(request.ProfileImageId);
                    if (result.IsFailure)
                    {
                        return Result.Failure<Guid>(Error.ValueInvalid(result.Error.Message));
                    }

                    facultyMember.ProfileImageId = result.Data;
                    //Strategy
                }
                else
                {
                    facultyMember.ProfileImageId = null;
                }
                    _context.FacultyMembers.Update(facultyMember);
                await _context.SaveChangesAsync();
                return Result.Success(facultyMember.Id);
            }
        }
    }

    public static class ProfileUpdateStrategyFactory
    {
        public static IProfileUpdateStrategy Create(string userType, ApplicationDbContext context, IGitHubService FileManager)
        {
            return userType switch
            {
                "Student" => new StudentUpdateStrategy(context, FileManager),
                "Faculty" => new FacultyUpdateStrategy(context, FileManager),
                _ => throw new ArgumentException($"Unsupported user type: {userType}", nameof(userType))
            };
        }
    }
}

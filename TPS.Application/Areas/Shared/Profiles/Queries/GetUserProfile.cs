using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Profiles.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Profiles.Queries;


public class GetUserProfile
{
    public class Query : IQuery<Result<UserProfileDto>>
    {
        public Query(Guid userId, UserType userType)
        {
            UserId = userId;
            UserType = userType;
        }

        public Guid UserId { get; set; }
        public UserType UserType { get; set; }

        public Query Create(Guid userId, UserType userType)
        {
            return new Query(userId, userType);
        }
    }


    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<UserProfileDto>>
    {

        public Task<Result<UserProfileDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request.UserType == UserType.Student)
            {
                var student = _context.Students
                                      .AsNoTracking()
                                      .Include(s => s.Department)
                                        .ThenInclude(s => s.School)
                                      .Include(s => s.SocietiesMembers)
                                        .ThenInclude(s => s.Society)
                                      .FirstOrDefault(s => s.Id == request.UserId);

                if (student is null || !student.IsActive)
                    return Task.FromResult(Result.Failure<UserProfileDto>(Error.NotFound(nameof(Student), request.UserId.ToString())));

                var result = GetStudentProfileDto(student);

                return Task.FromResult(Result.Success(result));
            }

            if (request.UserType == UserType.FacultyMember)
            {
                var facultyMember = _context.FacultyMembers
                                      .AsNoTracking()
                                      .Include(s => s.Department)
                                        .ThenInclude(s => s.School)
                                      .Include(s => s.SocietiesAdvised)
                                      .FirstOrDefault(s => s.Id == request.UserId);

                if (facultyMember is null || !facultyMember.IsActive)
                    return Task.FromResult(Result.Failure<UserProfileDto>(Error.NotFound(nameof(FacultyMember), request.UserId.ToString())));

                var result = GetFacultyMemberProfileDto(facultyMember);

                return Task.FromResult(Result.Success(result));
            }

            return Task.FromResult(Result.Failure<UserProfileDto>(Error.NotFound(nameof(ApplicationUser), request.UserId.ToString())));
        }


        private UserProfileDto GetStudentProfileDto(Student student)
        {
            return new UserProfileDto
            {
                Id = student.Id,
                Number = student.UniversityNumber,
                FullName = student.FirstName + " " + student.LastName,
                Email = student.Email,
                ProfileImageId = student.ProfileImageId,
                userType = UserType.Student,
                Department = student.Department?.Name,
                School = student.Department?.School?.Name,
                Memberships = student.SocietiesMembers.Select(sm => new MembershipBasicDetailsDto
                {
                    Section = sm.Position,
                    SocietyName = sm.Society.Name,
                    SocietyLogoId = sm.Society.LogoId,
                    JoinDate = sm.JoinDate
                }).ToList()
            };
        }

        private UserProfileDto GetFacultyMemberProfileDto(FacultyMember facultyMember)
        {
            return new UserProfileDto
            {
                Id = facultyMember.Id,
                Number = facultyMember.EmployeeNumber,
                FullName = facultyMember.FirstName + " " + facultyMember.LastName,
                Email = facultyMember.Email,
                ProfileImageId = facultyMember.ProfileImageId,
                userType = UserType.FacultyMember,
                Department = facultyMember.Department?.Name,
                School = facultyMember.Department?.School?.Name,
                Memberships = facultyMember.SocietiesAdvised.Select(s => new MembershipBasicDetailsDto
                {
                    Section = "Adviser",
                    SocietyName = s.Name,
                    SocietyLogoId = s.LogoId,
                    JoinDate = DateOnly.MinValue
                }).ToList()
            };
        }
    }
}
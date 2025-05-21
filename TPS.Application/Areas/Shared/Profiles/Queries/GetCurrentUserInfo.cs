using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Profiles.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Profiles.Queries;

public class GetCurrentUserInfo
{
    public class Query : IQuery<Result<CurrentUserDto>>
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


    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<CurrentUserDto>>
    {
        public async Task<Result<CurrentUserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if(request.UserType == UserType.Student)
            {
                var student = await _context.Students
                    .Where(x => x.Id == request.UserId)
                    .Select(x => new CurrentUserDto
                    {
                        Id = x.Id,
                        FullName = $"{x.FirstName} {x.LastName}",
                        Email = x.Email!,
                        Number = x.UniversityNumber,
                        ProfileImageId = x.ProfileImageId,
                        userType = request.UserType,
                        DepartmentId = x.DepartmentId,
                    })
                    .FirstOrDefaultAsync();

                return student is not null 
                    ? Result.Success(student)
                    : Result.Failure<CurrentUserDto>(Error.NotFound(nameof(Student), request.UserId.ToString()));
            }
            else if(request.UserType == UserType.FacultyMember)
            {
                var fmember = await _context.FacultyMembers
                    .Where(x => x.Id == request.UserId)
                    .Select(x => new CurrentUserDto
                    {
                        Id = x.Id,
                        FullName = $"{x.FirstName} {x.LastName}",
                        Email = x.Email!,
                        Number = x.EmployeeNumber,
                        ProfileImageId = x.ProfileImageId,
                        userType = request.UserType,
                        DepartmentId = x.DepartmentId,
                    })
                    .FirstOrDefaultAsync();

                return fmember is not null 
                    ? Result.Success(fmember)
                    : Result.Failure<CurrentUserDto>(Error.NotFound(nameof(FacultyMember), request.UserId.ToString()));
            }
            
            return Result.Failure<CurrentUserDto>(Error.ValueInvalid("UserType", request.UserType.ToString()));
        }

    }
}

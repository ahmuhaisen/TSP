using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Abstractions;

public interface IStudentsService
{
    Task<Result<Guid>> addCommitte(Guid StudentId, Guid SocietyId, AddCommitteeRequest request);
    Task<Result<Guid>> editMember(Guid studentId, Guid societyId, string position);
    Task<Result> deleteMember(Guid memberId, Guid SocietyId);
    Task<Result<List<MembersListDTO>>> getSocietyMembers(Guid SocietyId, bool IsCommittee);

}

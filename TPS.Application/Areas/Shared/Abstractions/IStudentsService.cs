using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts.Requests;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Abstractions;

public interface IStudentsService
{
    Task<Result<Guid>> addCommitte(AddCommitteeRequest request);
    Task<Result<Guid>> editMember(EditMemberRequest request);
    Task<Result> deleteMember(Guid memberId, Guid SocietyId);
    Task<Result<List<MembersListDTO>>> getSocietyMembers(Guid SocietyId, bool IsCommittee);

}

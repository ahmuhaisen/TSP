using TPS.Application.Areas.SuperAdmin.Contracts;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.SuperAdmin;


public interface IAccountsService
{
    Task<Result<List<PendingAccountBasicDto>>> GetPendingAccountsAsync();
    Task<Result> ApproveAccountAsync(Guid id, UserType userType);
    Task<Result> RejectAccountAsync(Guid id, UserType userType);
}

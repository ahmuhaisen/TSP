using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.SuperAdmin;
using TSP.Domain.Enums;

namespace TSP.WebAPI.Controllers.SuperAdminArea;


[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route($"api/[controller]")]
public class AccountsController(ISender sender, IAccountsService _accountsService) : ApiController(sender)
{

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingAccounts()
    {
        var task = _accountsService.GetPendingAccountsAsync();

        return await FromResult(task);
    }

    [HttpPut("approve/{id}")]
    public async Task<IActionResult> ApproveAccount(Guid id, [FromQuery] UserType userType)
    {
        var task = _accountsService.ApproveAccountAsync(id, userType);

        return await FromResult(task);
    }

    [HttpPut("reject/{id}")]
    public async Task<IActionResult> RejectAccount(Guid id, [FromQuery] UserType userType)
    {
        var task = _accountsService.RejectAccountAsync(id, userType);

        return await FromResult(task);
    }
}

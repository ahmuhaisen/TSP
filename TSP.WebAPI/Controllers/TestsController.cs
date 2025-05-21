using Microsoft.AspNetCore.Mvc;
using TPS.Infrastructure.AiClient;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Data.DataGenerators;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;

namespace TSP.WebAPI.Controllers;


[ApiController]
[Route($"api/[controller]")]
public class TestsController(ApplicationDbContext _context, IEmailService emailService, IGitHubService gitHubService, IAiClientService _aiClientService) : ControllerBase
{
    [HttpPost("GenerateFakeData")]
    public async Task<IActionResult> GenerateFakeData(int number = 5)
    {
        var data = new FacultyMemberFaker().Generate(number);

        foreach (var row in data)
        {
            _context.Add(row);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("emails/send")]
    public async Task<IActionResult> SendFakeEmail(string? subject)
    {
        try
        {

            //await emailService.SendWelcomingEmail("ahmuhaisen03@gmail.com", "Ahmad Muhaisen", UserType.FacultyMember);
            //await emailService.SendWelcomingEmail("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student);
            //await emailService.SendCommitteeChangesAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student,"ACM JU","Yasmin Almohtaseb",true);
            //await emailService.SendCommitteeChangesAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student,"ACM JU","Ahmad Muhaisen",false);
            //await emailService.SendCommitteeChangesAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.FacultyMember,"ACM JU","Ahmad Muhaisen",false);
            //await emailService.SendNewSocietyCreatedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.FacultyMember, "ACM JU");
            //await emailService.SendNewSocietyCreatedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU");
            //await emailService.SendEventRequestDecisionMade("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU","Junior To Solver",true,null);
            //await emailService.SendEventRequestDecisionMade("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU","Junior To Solver",false,"The date mentioned is an official holiday");
            //await emailService.SendNewEventScheduled("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU", "Junior To Solver");
            //await emailService.SendNewEventScheduled("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.FacultyMember, "ACM JU", "Junior To Solver");
            //await emailService.SendNewEventRequestSubmittedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU", "Junior To Solver");
            //await emailService.SendNewEventRequestSubmittedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.FacultyMember, "ACM JU", "Junior To Solver");
            //await emailService.SendMemberLeftTheSocietyAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU","Ahmad Muhaisen");
            //await emailService.SendNewMemberJoinedTheSocietyAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU","Ahmad Muhaisen");
            //await emailService.SendSocietyAdvisorChangedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU","Heba Saadeh","Abdulbasit Assaf",false,false);
            //await emailService.SendSocietyAdvisorChangedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.FacultyMember, "ACM JU","Heba Saadeh","Abdulbasit Assaf",true,false);
            //await emailService.SendSocietyAdvisorChangedAlert("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.FacultyMember, "ACM JU","Heba Saadeh","Abdulbasit Assaf",false,true);
            //await emailService.SendSocietyJoinRequestDecisionMade("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU",false);
            //await emailService.SendSocietyJoinRequestDecisionMade("yasmeenalmohtaseb@gmail.com", "Yasmin Almohtaseb", UserType.Student, "ACM JU",true);


            await emailService.SendWelcomingEmail("suhibsaleh@outlook.com", "Suhib Saleh", UserType.FacultyMember);
        }
        catch (Exception ex)
        {
            throw;
        }

        return Ok();
    }


    [HttpGet("ai/ask")]
    public async Task<IActionResult> AskAiAsync(string prompt)
    {
        try
        {
            return Ok(await _aiClientService.GetResponseAsync(prompt));
        }
        catch (Exception ex)
        {
            throw;
        }

        return Ok();
    }
}

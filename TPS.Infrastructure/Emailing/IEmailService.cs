using TSP.Domain.Enums;

namespace TPS.Infrastructure.Emailing;

public interface IEmailService
{
    Task Send(string to, string subject, string body);
    Task SendWelcomingEmail(string to, string userName, UserType userType);
    
    Task SendNewSocietyCreatedAlert(string to, string userName, UserType userType, string societyName);

    Task SendEventRequestDecisionMade(string to, string userName, UserType userType,string societyName, string eventName,bool decision,string? remark);
    Task SendNewEventScheduled(string to, string userName, UserType userType, string societyName, string eventName);
    Task SendNewEventRequestSubmittedAlert(string to, string userName, UserType userType, string societyName, string eventName);

    Task SendMemberLeftTheSocietyAlert(string to, string userName, UserType userType, string societyName, string userNameLeft);
    Task SendNewMemberJoinedTheSocietyAlert(string to,string userName,UserType userType,string societyName, string userNameJoined);

    Task SendSocietyAdvisorChangedAlert(string to, string userName, UserType userType, string societyName, string newAdvisorName,string oldAdvisorName,bool isNewAdvisor,bool isOldAdvisor);

    Task SendCommitteeChangesAlert(string to, string userName, UserType userType, string societyName, string committeeName,bool isSameUser);

    Task SendSocietyJoinRequestDecisionMade(string to, string userName, UserType userType, string societyName, bool decision);
}
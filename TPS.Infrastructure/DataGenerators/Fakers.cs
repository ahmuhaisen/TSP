using Bogus;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.DataGenerators;

public class StudentFaker : Faker<Student>
{
    public StudentFaker()
    {
        RuleFor(s => s.Id, f => Guid.NewGuid());
        RuleFor(s => s.UserName, f => f.Internet.UserName());
        RuleFor(s => s.NormalizedUserName, (f, s) => s.UserName.ToUpperInvariant());
        RuleFor(s => s.Email, (f, fm) => $"{fm.UserName.ToLower()}@gmail.com");
        RuleFor(s => s.NormalizedEmail, (f, s) => s.Email.ToUpperInvariant());
        RuleFor(s => s.EmailConfirmed, f => true);
        RuleFor(s => s.PasswordHash, f => "Aa123*");
        RuleFor(s => s.SecurityStamp, f => Guid.NewGuid().ToString());
        RuleFor(s => s.ConcurrencyStamp, f => Guid.NewGuid().ToString());
        RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber());
        RuleFor(s => s.PhoneNumberConfirmed, f => true);
        RuleFor(s => s.TwoFactorEnabled, f => false);
        RuleFor(s => s.LockoutEnabled, f => false);
        RuleFor(s => s.LockoutEnd, f => null);
        RuleFor(s => s.AccessFailedCount, f => 0);

        // Custom properties for Student
        RuleFor(f => f.FirstName, f => f.Name.FirstName());
        RuleFor(f => f.LastName, f => f.Name.LastName());
        RuleFor(s => s.DepartmentId, f => f.PickRandom(new[] { 1, 2, 3, 4 }));
        RuleFor(s => s.UniversityNumber, f => f.Random.AlphaNumeric(8).ToUpper());
        //RuleFor(s => s.SocietiesMembers, f => new SocietiesMembersFaker().Generate(2));
    }

    private string GenerateFakePasswordHash(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha256.ComputeHash(bytes));
    }
}


public class SocietiesMembersFaker : Faker<SocietiesMembers>
{
    private List<Guid> societyIds = new List<Guid>
        {
            Guid.Parse("7D020FEE-2D6A-49EA-F36B-08DD179C8CA2"),
        };

    private List<Guid> studentIds = new List<Guid>
        {
            Guid.Parse("74A0A6F4-0E09-487F-BA63-09002C7A1A49"),
            
        };

    public SocietiesMembersFaker()
    {
        RuleFor(sm => sm.SocietyId, f => f.PickRandom(societyIds));
        RuleFor(sm => sm.StudentId, f => f.PickRandom(studentIds));
        RuleFor(sm => sm.JoinDate, f => DateOnly.FromDateTime(f.Date.Past(2)));
        RuleFor(sm => sm.IsActive, f => true);
    }
}

public class FacultyMemberFaker : Faker<FacultyMember>
{
    public FacultyMemberFaker()
    {
        RuleFor(fm => fm.Id, f => Guid.NewGuid());
        RuleFor(fm => fm.UserName, f => f.Internet.UserName());
        RuleFor(fm => fm.NormalizedUserName, (f, fm) => fm.UserName.ToUpperInvariant());
        RuleFor(fm => fm.Email, (f, fm) => $"{fm.UserName.ToLower()}@gmail.com");
        RuleFor(fm => fm.NormalizedEmail, (f, fm) => fm.Email.ToUpperInvariant());
        RuleFor(fm => fm.EmailConfirmed, f => true);
        RuleFor(fm => fm.PasswordHash, f => "Aa123*");
        RuleFor(fm => fm.SecurityStamp, f => Guid.NewGuid().ToString());
        RuleFor(fm => fm.ConcurrencyStamp, f => Guid.NewGuid().ToString());
        RuleFor(fm => fm.PhoneNumber, f => f.Phone.PhoneNumber());
        RuleFor(fm => fm.PhoneNumberConfirmed, f => false);
        RuleFor(fm => fm.TwoFactorEnabled, f => false);
        RuleFor(fm => fm.LockoutEnabled, f => false);
        RuleFor(fm => fm.LockoutEnd, f => null);
        RuleFor(fm => fm.AccessFailedCount, f => 0);

        // FacultyMember-specific properties
        RuleFor(f => f.FirstName, f => f.Name.FirstName());
        RuleFor(f => f.LastName, f => f.Name.LastName());
        RuleFor(fm => fm.EmployeeNumber, f => f.Random.AlphaNumeric(6).ToUpper());
        RuleFor(fm => fm.RankId, f => f.Random.Int(1, 7));
        RuleFor(fm => fm.DepartmentId, f => f.Random.Int(1, 4));
        //RuleFor(fm => fm.SocietiesAdvised, f => new SocietyFaker().Generate(2));
    }

    private string GenerateFakePasswordHash(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha256.ComputeHash(bytes));
    }
}


public static class FacultyRankData
{
    public static List<Rank> GetPredefinedRanks()
    {
        return new List<Rank>
        {
             new Rank { Id = 1, Title = "Professor" },
             new Rank { Id = 2, Title = "Associate Professor" },
             new Rank { Id = 3, Title = "Assistant Professor" },
             new Rank { Id = 4, Title = "Teacher" },
             new Rank { Id = 5, Title = "Department Chair" },
             new Rank { Id = 6, Title = "Dean" },
             new Rank { Id = 7, Title = "Dean Assistant" }
        };
    }
}

public static class DepartmentData
{
    public static List<Department> GetPredefinedRanks()
    {
        return new List<Department>
        {
            new Department { Id = 1, Name = "Computer Science", Abbreviation = "CS" },
            new Department { Id = 2, Name = "Computer Information Systems", Abbreviation = "CIS" },
            new Department { Id = 3, Name = "Information Technology", Abbreviation = "IT" },
            new Department { Id = 4, Name = "Artificial Intelligence", Abbreviation = "AI" }
        };
    }
}

public class SocietyFaker : Faker<Society>
{
    private static readonly List<Guid> AdvisorIds = new List<Guid>
    {
        new Guid("8725B75E-47A5-4923-0BE7-08DD3FBB891A")
    };

    public SocietyFaker()
    {
        RuleFor(s => s.Name, f => f.Company.CompanyName());
        RuleFor(s => s.Description, f => f.Lorem.Sentence(10));
        RuleFor(s => s.LogoId, f => $"{Guid.NewGuid().ToString().Substring(0, 5)}.png");
        RuleFor(s => s.CreationDate, f => DateOnly.FromDateTime(f.Date.Past(10)));
        RuleFor(s => s.ThemeColor, f => f.Internet.Color());

        // Pick a random advisor ID from the list of predefined GUIDs
        RuleFor(s => s.AdvisorId, f => f.PickRandom(AdvisorIds));
    }
}

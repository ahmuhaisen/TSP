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
        RuleFor(s => s.Email, f => f.Internet.Email());
        RuleFor(s => s.NormalizedEmail, (f, s) => s.Email.ToUpperInvariant());
        RuleFor(s => s.EmailConfirmed, f => f.Random.Bool());
        RuleFor(s => s.PasswordHash, f => GenerateFakePasswordHash("Aa123*"));
        RuleFor(s => s.SecurityStamp, f => Guid.NewGuid().ToString());
        RuleFor(s => s.ConcurrencyStamp, f => Guid.NewGuid().ToString());
        RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber());
        RuleFor(s => s.PhoneNumberConfirmed, f => f.Random.Bool());
        RuleFor(s => s.TwoFactorEnabled, f => f.Random.Bool());
        RuleFor(s => s.LockoutEnabled, f => f.Random.Bool());
        RuleFor(s => s.LockoutEnd, f => f.Random.Bool() ? (DateTimeOffset?)f.Date.Future() : null);
        RuleFor(s => s.AccessFailedCount, f => f.Random.Int(0, 5));

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
            Guid.Parse("356C8FB2-78EE-4BAB-F36C-08DD179C8CA2"),
            Guid.Parse("AF548BA8-67E0-43D6-F36D-08DD179C8CA2"),
            Guid.Parse("90724BEB-4E97-4613-F36E-08DD179C8CA2"),
            Guid.Parse("8843079C-E6C8-4AD9-F36F-08DD179C8CA2"),
            Guid.Parse("B846B39E-D644-4500-F370-08DD179C8CA2"),
            Guid.Parse("9CAF6F58-769B-4337-F371-08DD179C8CA2"),
            Guid.Parse("A4ECE814-8945-4E15-F372-08DD179C8CA2"),
            Guid.Parse("C0F842CF-FACF-42AE-F373-08DD179C8CA2"),
            Guid.Parse("A3A1D09B-77E0-497F-F374-08DD179C8CA2"),
            Guid.Parse("CB9016AE-4A52-4175-F375-08DD179C8CA2"),
            Guid.Parse("BCB68CCA-A729-4FB9-F376-08DD179C8CA2"),
            Guid.Parse("6ED24F1D-098C-4224-F377-08DD179C8CA2"),
            Guid.Parse("539EE0E1-2F91-483E-F378-08DD179C8CA2"),
            Guid.Parse("4BD56784-46F3-4216-F379-08DD179C8CA2"),
            Guid.Parse("4632701A-07DD-417C-F37A-08DD179C8CA2"),
            Guid.Parse("90A28E06-505B-4CA8-F37B-08DD179C8CA2"),
            Guid.Parse("840CD792-503C-42F7-F37C-08DD179C8CA2"),
            Guid.Parse("55317D26-D002-4FE5-F37D-08DD179C8CA2"),
            Guid.Parse("58EFE0F4-8DF7-428E-F37E-08DD179C8CA2"),
            Guid.Parse("2B953C96-59FC-4691-F37F-08DD179C8CA2"),
            Guid.Parse("ED613BCE-E83B-4063-F380-08DD179C8CA2"),
            Guid.Parse("3764CF1D-340F-4CC6-F381-08DD179C8CA2"),
            Guid.Parse("6A0E5797-77DD-4930-F382-08DD179C8CA2"),
            Guid.Parse("54CB9070-D0A8-4AFF-F383-08DD179C8CA2"),
            Guid.Parse("B16D2A51-C5EB-4F7C-F384-08DD179C8CA2"),
            Guid.Parse("12F9C294-6A66-4A1A-F385-08DD179C8CA2"),
            Guid.Parse("1840614D-0659-4D02-F386-08DD179C8CA2"),
            Guid.Parse("E3ACBECA-ED8B-4025-F387-08DD179C8CA2"),
            Guid.Parse("3EBF0E95-F0D4-48A2-F388-08DD179C8CA2")
        };

    private List<Guid> studentIds = new List<Guid>
        {
            Guid.Parse("74A0A6F4-0E09-487F-BA63-09002C7A1A49"),
            Guid.Parse("C8C7EB6A-B735-4B73-B41E-0DE9B250DF6E"),
            Guid.Parse("95E22F29-C749-4FA6-ADC3-11F1616E4E53"),
            Guid.Parse("383A0A0A-0EC0-4488-B1A0-13659EBB5556"),
            Guid.Parse("A5F27F6C-1A36-44C3-AFF8-14D068CB60D2"),
            Guid.Parse("79751DA6-1A9E-4B1E-8BA1-14E0573939F9"),
            Guid.Parse("6B420BF9-0248-44F9-8131-1527C3DFD966"),
            Guid.Parse("9AEB8D14-2108-4E16-877A-170AA59EE0DA"),
            Guid.Parse("DFEE350A-DCA2-4493-BE96-1914872D4D38"),
            Guid.Parse("5F82D4BE-591B-4A67-B269-1B3DAB98B6B3"),
            Guid.Parse("A68CA969-9F88-40E6-9132-1EA67F980CFD"),
            Guid.Parse("6E1F5C76-2C0F-41F2-8388-20CFF62A07F8"),
            Guid.Parse("92A7508D-AEEF-492C-B747-20F865F971A6"),
            Guid.Parse("1ED6E86F-F957-4980-A17D-21346F9D17C0"),
            Guid.Parse("B5F6E7D5-1E85-4FEF-86BD-22171C4BA179"),
            Guid.Parse("64E0B86D-FC1D-49CC-BB46-228F20514B91"),
            Guid.Parse("777F0222-C482-4FDB-904A-2381FC6DC97C"),
            Guid.Parse("2C929690-1844-458D-BAD2-24D82CBED982"),
            Guid.Parse("37A0657F-324C-4DB2-BF26-26F58AD907AA"),
            Guid.Parse("6610320E-1402-4AFC-8F85-26FDDF4B3884"),
            Guid.Parse("62DAEF31-B7C3-440E-82BE-2946318C4F46"),
            Guid.Parse("BA6A2271-3066-42D7-A937-29CDCBACF0D5"),
            Guid.Parse("B199CFE9-E051-492F-B1B1-2A4414C40262"),
            Guid.Parse("4413463A-BA3A-4212-B48A-2DDBB5B13A90"),
            Guid.Parse("1AE031D5-CBC0-408E-A3C6-2E7408ED9BAA"),
            Guid.Parse("33A7CE27-3D69-4292-A967-2E85300761E7"),
            Guid.Parse("BB9D1BC2-900E-4C1C-927A-2ECB21288042"),
            Guid.Parse("5A148239-0364-4551-9501-2F5199708204"),
            Guid.Parse("279416CD-8FD5-4B48-9C63-308F4C08E822"),
            Guid.Parse("2424176F-4EBE-4B22-A68D-312D82C3DFE3"),
            Guid.Parse("C7580BD8-2EB0-43C5-A292-326FEB491144"),
            Guid.Parse("3EF6D7C7-A11A-4D3E-9AC5-32D9D06A75C1"),
            Guid.Parse("43C3D378-0A8F-450E-8B15-33F508D15460"),
            Guid.Parse("27DEB6E7-B272-46F6-83B6-356BD7D62B32"),
            Guid.Parse("B791445F-2E2A-47DC-9AC9-358296AABBED"),
            Guid.Parse("5C8E6CAB-6675-4E52-804D-35DE802222CB"),
            Guid.Parse("E170AA1C-12B3-4C5C-81E8-3618ECFEFA87"),
            Guid.Parse("7F18F639-01A3-4765-BF11-36F837A46738"),
            Guid.Parse("C03D8302-EDB6-4A9E-9F06-373AC7C67B25"),
            Guid.Parse("8FA9ECB5-B6AF-499A-BE11-37CF75510A99"),
            Guid.Parse("0F68BB40-605F-47A3-95A9-3B0A0BB15CBB"),
            Guid.Parse("44AD0FA8-FA55-44FF-BFBF-3B7FFAEA6BAC"),
            Guid.Parse("B1E825E3-7026-4F3F-8E17-3BB7EA75F2E5"),
            Guid.Parse("FE07C3B0-5176-4590-9C83-3C44DEE44069"),
            Guid.Parse("49251D86-1842-496C-A5C8-3DB309C07AC3"),
            Guid.Parse("0D1DBE74-E062-4EA2-A1D3-4020710BC1EF"),
            Guid.Parse("A18BC5B8-20C6-4956-ADCB-435E0602FC25"),
            Guid.Parse("BD4CECD7-9C9B-4E07-B92D-4438BCE0C5C0"),
            Guid.Parse("952EA8F4-5B92-4DDC-8618-448EEDC538D0"),
            Guid.Parse("0656A047-BBE8-4A51-A565-45C3542EB95D"),
            Guid.Parse("E28AC8D2-329A-45CF-9A6B-4673175274D1"),
            Guid.Parse("FA16009D-5165-4DFF-99EF-47113F778971"),
            Guid.Parse("F85990F1-20A5-41E0-9896-4AFB84B87998"),
            Guid.Parse("B3ED379A-0EDB-43F7-9541-4BBC78CFF287"),
            Guid.Parse("1EC18F6C-FFB9-471C-A2AF-4CAAEBAF92C6"),
            Guid.Parse("EC98AD38-19A9-44F7-B5F7-4D458A93E0E5"),
            Guid.Parse("20204F2C-2FBE-47F6-B088-50451FEC34F6"),
            Guid.Parse("68A60F55-C712-484B-9F80-50E6541B6BB3"),
            Guid.Parse("7273A502-93E9-4BC3-B663-528B48F66AB6"),
            Guid.Parse("6ABD1A92-BA28-41F0-A120-54FE2FC473DD"),
            Guid.Parse("4BCE9B56-4B43-46C8-8551-55B7097F7F8A"),
            Guid.Parse("A54227D9-43BE-41C8-B11D-55F512448764"),
            Guid.Parse("029F0C2C-B149-4A99-8257-5DE3AD06F08E"),
            Guid.Parse("C259756E-E6C1-4958-9CF3-61171E545E14"),
            Guid.Parse("C4B4397F-3F6F-49C2-8354-62FFF73D1D96")
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
        RuleFor(fm => fm.Email, f => f.Internet.Email());
        RuleFor(fm => fm.NormalizedEmail, (f, fm) => fm.Email.ToUpperInvariant());
        RuleFor(fm => fm.EmailConfirmed, f => f.Random.Bool());
        RuleFor(fm => fm.PasswordHash, f => GenerateFakePasswordHash("SecurePassword123!"));
        RuleFor(fm => fm.SecurityStamp, f => Guid.NewGuid().ToString());
        RuleFor(fm => fm.ConcurrencyStamp, f => Guid.NewGuid().ToString());
        RuleFor(fm => fm.PhoneNumber, f => f.Phone.PhoneNumber());
        RuleFor(fm => fm.PhoneNumberConfirmed, f => f.Random.Bool());
        RuleFor(fm => fm.TwoFactorEnabled, f => f.Random.Bool());
        RuleFor(fm => fm.LockoutEnabled, f => f.Random.Bool());
        RuleFor(fm => fm.LockoutEnd, f => f.Random.Bool() ? (DateTimeOffset?)f.Date.Future() : null);
        RuleFor(fm => fm.AccessFailedCount, f => f.Random.Int(0, 5));

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

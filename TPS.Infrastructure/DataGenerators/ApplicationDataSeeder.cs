using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.DataGenerators;

public class ApplicationDataSeeder(ApplicationDbContext _context, RoleManager<ApplicationRole> _roleManager)
{
    public async Task Seed()
    {
        await seedRoles();
        await seedFacultyMembers();
        await seedSocieties();
        await seedStudents();
        await seedStudentMembership();
    }

    private async Task seedRoles()
    {
        string[] roles = { "Student", "Faculty" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }

    private async Task seedFacultyMembers()
    {
        if (_context.FacultyMembers.Any())
        {
            return;
        }

        List<FacultyMember> data = [
            new FacultyMember
            {
                Id = new Guid("B6530ABE-BFE5-4212-8ACB-08DD5E35E650"),
                FirstName = "Abdelbast",
                LastName = "A'asaf",
                Email = "asaf@ju.edu.jo",
                UserName = "asaf",
                DepartmentId = 2,
                Gender = Gender.Male,
                PhoneNumber = "0799999999",
                PasswordHash = "AQAAAAIAAYagAAAAELIhT49xTHOXI0y72eDfBfyjO2rS+RgWK4USYS3KSxFT8aC2IR8o0MsLuc7n/o+Mxg==",
                SecurityStamp = "76ZUI2EVMGSBUF7NWRTDDMRYSD3P2OZA",
                EmployeeNumber = "CIS01",
                RankId = 2
            },
            new FacultyMember
            {
                Id = new Guid("E67BB4A6-0EB9-498A-8ACD-08DD5E35E650"),
                FirstName = "Musa",
                LastName = "Al Akhras",
                Email = "musa@ju.edu.jo",
                UserName = "musa",
                DepartmentId = 2,
                Gender = Gender.Male,
                PhoneNumber = "0799999999",
                PasswordHash = "AQAAAAIAAYagAAAAELIhT49xTHOXI0y72eDfBfyjO2rS+RgWK4USYS3KSxFT8aC2IR8o0MsLuc7n/o+Mxg==",
                SecurityStamp = "76ZUI2EVMGSBUF7NWRTDDMRYSD3P2OZA",
                EmployeeNumber = "CIS02",
                RankId = 4
            },
            new FacultyMember
            {
                Id = new Guid("75445A2D-EC45-4E52-8ACC-08DD5E35E650"),
                FirstName = "Ruba",
                LastName = "E'baidat",
                Email = "ruba@ju.edu.jo",
                UserName = "ruba",
                DepartmentId = 4,
                Gender = Gender.Female,
                PhoneNumber = "0799999999",
                PasswordHash = "AQAAAAIAAYagAAAAELIhT49xTHOXI0y72eDfBfyjO2rS+RgWK4USYS3KSxFT8aC2IR8o0MsLuc7n/o+Mxg==",
                SecurityStamp = "76ZUI2EVMGSBUF7NWRTDDMRYSD3P2OZA",
                EmployeeNumber = "AIS01",
                RankId = 4
            }
            ];

        foreach (var facultyMember in data)
        {
            facultyMember.PasswordHash = passwordHasher(facultyMember);
            facultyMember.NormalizedEmail = facultyMember.Email.ToUpperInvariant();
            facultyMember.NormalizedUserName = facultyMember.UserName.ToUpperInvariant();
            facultyMember.EmailConfirmed = true;
        }

        _context.FacultyMembers.AddRange(data);
        await _context.SaveChangesAsync();
    }

    private async Task seedSocieties()
    {
        if (_context.Societies.Any())
        {
            return;
        }

        List<Society> data = [
            new Society
            {
                Id = Guid.Parse("7981a758-5274-4349-ba71-6b8e689e9ea9"),
                Name = "ACM JU Student Chapter",
                Description = "A society for Problem Solving.",
                LogoId = string.Empty,
                CreationDate = new DateOnly(2017, 1, 1),
                ThemeColor = "#FF0000",
                AdvisorId = new Guid("B6530ABE-BFE5-4212-8ACB-08DD5E35E650")
            },
            new Society
            {
                Id = Guid.Parse("2a077a71-972d-4b6f-80e5-f2103dafd753"),
                Name = "Waves JU",
                Description = "A society for Robotics.",
                LogoId = string.Empty,
                CreationDate = new DateOnly(2024, 1, 1),
                ThemeColor = "#FF0000",
                AdvisorId = new Guid("75445A2D-EC45-4E52-8ACC-08DD5E35E650")
            },
            new Society
            {
                Id = Guid.Parse("6f5fbae1-d89a-4dbd-96cd-7e7929cde69a"),
                Name = "IEEE CS JU",
                Description = "A society for Computer Science students.",
                LogoId = string.Empty,
                CreationDate = new DateOnly(2019, 1, 1),
                ThemeColor = "#FF0000",
                AdvisorId = new Guid("E67BB4A6-0EB9-498A-8ACD-08DD5E35E650")
            },
            ];

        _context.Societies.AddRange(data);
        await _context.SaveChangesAsync();
    }

    private async Task seedStudents()
    {
        if(_context.Students.Any())
        {
            return;
        }

        List<Student> data = new StudentFaker().Generate(50);

        foreach (Student student in data)
        {
            student.PasswordHash = passwordHasher(student);
            student.NormalizedEmail = student.Email.ToUpperInvariant();
            student.NormalizedUserName = student.UserName.ToUpperInvariant();
            student.EmailConfirmed = true;
        }

        _context.Students.AddRange(data);
        await _context.SaveChangesAsync();
    }

    private async Task seedStudentMembership()
    {
        if (_context.SocietiesMembers.Any())
        {
            return;
        }

        List<SocietiesMembers> data = [];

        var society = await _context.Societies.FirstAsync();
        var students = _context.Students.ToList();
        for (var i = 0; i < students.Count(); i++)
        {
            var student = students[i];
            data.Add(new SocietiesMembers
            {
                SocietyId = society.Id,
                StudentId = student.Id,
                JoinDate = new DateOnly(2021, 1, 1),
                IsActive = true,
                Position = i % 10 == 0 ? $"Committee {i / 10 + 1}" : "Member",
                IsCommittee = i % 10 == 0
            });
        }

        _context.SocietiesMembers.AddRange(data);
        await _context.SaveChangesAsync();
    }

    private string passwordHasher<T>(T user) where T : ApplicationUser
    {
        var password = "Aa123*";
        var passwordHasher = new PasswordHasher<T>();

        return passwordHasher.HashPassword(user, password);
    }
}

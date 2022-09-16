using Bogus;
using LMS20.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LMS20.Data.Data
{
    public class SeedData
    {
        private static ApplicationDbContext db = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<ApplicationUser> userManager = default!;

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services, string teacherPW, string studentPW)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            db = context;

            if (db.Users.Any()) return;
               
            ArgumentNullException.ThrowIfNull(nameof(services));

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            ArgumentNullException.ThrowIfNull(roleManager);

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            ArgumentNullException.ThrowIfNull(userManager);

            var roleNames = new[] { "Student", "Teacher" };

            var teacherEmail = "larare@lms.se";
            var studentEmail = "student@lms.se";

            var course = GetCourse();
            await db.AddRangeAsync(course);

            //var modules = GetModules();
            //await db.AddRangeAsync(modules);

            //var moduleActivities = GetModuleActivities();
            //await db.AddRangeAsync(moduleActivities);

            await AddRolesAsync(roleNames);

            var teacher = await AddTeacherAsync(teacherEmail, teacherPW);

            await AddToRolesAsyncTeacher(teacher, roleNames);

            var student = await AddStudentAsync(studentEmail, studentPW);

            await AddToRoleAsyncStudent(student, "Student");

            await db.SaveChangesAsync();
        }

        // Lägger till lärare till rollen "Teacher" och "Student"
        private static async Task AddToRolesAsyncTeacher(ApplicationUser teacher, string[] roleNames)
        {
            foreach(var role in roleNames)
            {
                if (await userManager.IsInRoleAsync(teacher, role)) continue;
                var result = await userManager.AddToRoleAsync(teacher, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }


        // Lägger till student till rollen "Student"
        private static async Task AddToRoleAsyncStudent(ApplicationUser student, string roleName)
        {

                if (await userManager.IsInRoleAsync(student, roleName)) return;
                var result = await userManager.AddToRoleAsync(student, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));            
        }

        // Seedar en lärare

        private static async Task<ApplicationUser> AddTeacherAsync(string teacherEmail, string teacherPW)
        {

            var found = await userManager.FindByEmailAsync(teacherEmail);

            if (found != null) return null!;

            var teacher = new ApplicationUser
            {
                FirstName = "Lärare",
                LastName = "Läraresson",
                UserName = teacherEmail,
                Email = teacherEmail
            };

            var result = await userManager.CreateAsync(teacher, teacherPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return teacher;
        }

        // Seedar en student
        private static async Task<ApplicationUser> AddStudentAsync(string studentEmail, string studentPW)
        {
            var found = await userManager.FindByEmailAsync(studentEmail);

            if (found != null) return null!;

            var student = new ApplicationUser
            {
                FirstName = "Student",
                LastName = "Studentsson",
                UserName = studentEmail,
                Email = studentEmail
            };

            var result = await userManager.CreateAsync(student, studentPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return student;
        }

        // Seedar roller
        private static async Task AddRolesAsync(string [] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        // Seedar en kurs
        private static Course GetCourse()
        {
            var faker = new Faker("sv");

            var course = new Course
            {
                Name = "Programmering",
                Description = faker.Company.Bs(),
                StartDateTime = DateTime.Now,
                Duration = new TimeSpan(0, 55, 0),
                Modules = GetModules()

            };

            return course;
        }

        // Seedar moduler
        private static ICollection<Module> GetModules()
        {
            var faker = new Faker("sv");

            var modules = new List<Module>();

            ////var course = db.Module.Include(m => m.Course).ToListAsync();
            //var module = db.Module
            //    .Include(m => m.ModuleActivities).ThenInclude(ma => ma.Documents)
            //    .Include(m => m.Documents)
            //    .FirstOrDefault();

            for (int i = 0; i < 2; i++)
            {
                var module = new Module
                {
                    Name = "Modul 1",
                    Description = faker.Company.Bs(),
                    StartDateTime = DateTime.Now,
                    Duration = new TimeSpan(0, 55, 0),
                    //CourseId = course.Id
                    ModuleActivities = GetModuleActivities()
                    
                };

                modules.Add(module);
            }

            return modules;
        }

        // Seedar aktiviteter
        private static ICollection<ModuleActivity> GetModuleActivities()
        {
            var faker = new Faker("sv");

            var moduleActivities = new List<ModuleActivity>();

            //var module = db.ModuleActivity.Include(m => m.Module).ToListAsync();

            for(int i = 0; i < 2; i++)
            {
                var temp = new ModuleActivity
                {
                    Name = "Föreläsning",
                    Description = faker.Company.Bs(),
                    StartDateTime = DateTime.Now,
                    Duration = new TimeSpan(0, 55, 0),
                    //ModuleId = module.Id
                    //Type = "Lecture"
                };

                moduleActivities.Add(temp);
            }

            return moduleActivities;
        }
    }
}

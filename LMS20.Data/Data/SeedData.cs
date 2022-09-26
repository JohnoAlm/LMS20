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

            var studentPassword = "abc123";

            var courses = GetCourses().ToList();
            await db.AddRangeAsync(courses);
            await db.SaveChangesAsync();

            await AddRolesAsync(roleNames);

            await AddStudentsAsync(studentPassword, courses);
            //await db.AddRangeAsync(students);
            //await db.SaveChangesAsync();

            //var modules = GetModules();
            //await db.AddRangeAsync(modules);

            //var moduleActivities = GetModuleActivities();
            //await db.AddRangeAsync(moduleActivities);


            var teacher = await AddTeacherAsync(teacherEmail, teacherPW);
            await AddToRolesAsyncTeacher(teacher, roleNames);

        }

        // Lägger till lärare till rollen "Teacher" och "Student"
        private static async Task AddToRolesAsyncTeacher(ApplicationUser teacher, string[] roleNames)
        {
            foreach (var role in roleNames)
            {
                if (await userManager.IsInRoleAsync(teacher, role)) continue;
                var result = await userManager.AddToRoleAsync(teacher, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }


        // Lägger till student till rollen "Student"
        private static async Task AddToRoleAsyncStudent(ICollection<ApplicationUser> students, string roleName)
        {
            foreach (var student in students)
            {
                if (await userManager.IsInRoleAsync(student, roleName)) return;
                var result = await userManager.AddToRoleAsync(student, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
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

        // Seedar flera studenter
        private static async Task AddStudentsAsync(string studentPW, List<Course> courses)
        {
            var faker = new Faker("sv");

            var students = new List<ApplicationUser>();

            string firstName;
            string lastName;
            string userName;
            string email;

            foreach (var course in courses)
            {

                for (int i = 1; i < 4; i++)
                {
                    firstName = faker.Name.FirstName();
                    lastName = faker.Name.LastName();
                    userName = $"{firstName}.{lastName}@lms.se";
                    email = userName;

                    var student = new ApplicationUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = userName,
                        Email = email,
                        Course = course
                    };

                    var result = await userManager.CreateAsync(student, studentPW);
                    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                    students.Add(student);
                }
            }

            await AddToRoleAsyncStudent(students, "Student");

        }

        // Seedar roller
        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }


        // Seedar flera kurser
        private static ICollection<Course> GetCourses()
        {
            var faker = new Faker("sv");

            var courses = new List<Course>()
            {
                 new Course
                {
                    Name = "Grundkurs 101",
                    Description = faker.Company.Bs(),
                    Start = new DateTime(2022, 02, 16),
                    End = new DateTime(2022, 06, 15),
                    Modules =  new List<Module>()

                    {
                        new Module{
                        Name = "Modul old",
                        Description = faker.Company.Bs(),
                        Start = new DateTime(2022, 03, 16),
                        End = new DateTime(2022, 06, 15),
                        ModuleActivities = new List<ModuleActivity>()
                        {
                        new ModuleActivity{
                            Name = "Föreläsning old1",
                            Description = faker.Company.Bs(),
                            Start = new DateTime(2022, 03, 17, 09, 0,0 ),
                            End = new DateTime(2022, 03, 17, 11, 0,0 ),
                            ActivityType = ActivityType.Lecture
                            },
                         new ModuleActivity{
                            Name = "Uppgift old1",
                            Description = faker.Company.Bs(),
                            Start = new DateTime(2022, 05, 15, 09, 0,0 ),
                            End = new DateTime(2022, 05, 30, 18, 0,0 ),
                            ActivityType = ActivityType.Lecture
                         }
                        }
                    }
                        }
                 },//modullistan 
                     //nästa kurs

                 new Course
                {
                    Name = "Programmering 101",
                    Description = faker.Company.Bs(),
                     Start = new DateTime(2022, 08, 16),
                     End = new DateTime(2022, 10, 15),
                    Modules =  new List<Module>()

                    {
                     new Module
                      {
                        Name = "Modul 1",
                        Description = faker.Company.Bs(),
                        Start =new DateTime(2022, 09, 01 ),
                        End = new DateTime(2022, 10, 15),


                           ModuleActivities = new List<ModuleActivity>()
                           {
                            new ModuleActivity{
                                Name = "Föreläsning 101",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 26, 09, 0,0 ),
                                End = new DateTime(2022, 09, 26, 11, 0,0 ),
                                ActivityType = ActivityType.Lecture
                              },

                            new ModuleActivity{
                                Name = "Föreläsning 102",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 28, 09, 0,0 ),
                                End = new DateTime(2022, 09, 28, 11, 0,0 ),
                                ActivityType = ActivityType.Lecture
                            },
                            new ModuleActivity{
                                Name = "Föreläsning 103",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 29, 09, 0,0 ),
                                End = new DateTime(2022, 09, 29, 11, 0,0 ),
                                ActivityType = ActivityType.Lecture
                           },
                            new ModuleActivity{
                                Name = "Föreläsning 104",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 22, 09, 0,0 ),
                                End = new DateTime(2022, 09, 22, 11, 0,0 ),
                                ActivityType = ActivityType.Lecture
                           },
                            new ModuleActivity{
                                Name = "Föreläsning 105",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 23, 09, 0,0 ),
                                End = new DateTime(2022, 09, 23, 11, 0,0 ),
                                ActivityType = ActivityType.Lecture
                           },
                            new ModuleActivity{
                                Name = "Uppgift 100",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 01, 09, 0,0 ),
                                End = new DateTime(2022, 09, 15, 18, 0,0 ),
                                ActivityType = ActivityType.Task
                            },
                           new ModuleActivity{
                                Name = "Uppgift 101",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 01, 09, 0,0 ),
                                End = new DateTime(2022, 09, 28, 18, 0,0 ),
                                ActivityType = ActivityType.Task
                            },
                           new ModuleActivity{
                                Name = "Uppgift 102",
                                Description = faker.Company.Bs(),
                                Start = new DateTime(2022, 09, 01, 09, 0,0 ),
                                End = new DateTime(2022, 09, 30, 18, 0,0 ),
                                ActivityType = ActivityType.Task
                            }//modulakt
                           }//modulaktivlista
                      


                    },
                    new Module
                    {
                    Name = "Modul 2",
                    Description = faker.Company.Bs(),
                    Start =new DateTime(2022, 10, 16),
                    End = new DateTime(2022, 12, 15),
                    //CourseId = course.Id
                     ModuleActivities = new List<ModuleActivity>()
                     {
                    new ModuleActivity{
                        Name = "Föreläsning 201",
                        Description = faker.Company.Bs(),
                        Start = new DateTime(2022, 10, 15, 09, 0,0 ),
                        End = new DateTime(2022, 10, 15, 11, 0,0 ),
                        ActivityType = ActivityType.Lecture
                        },
                     new ModuleActivity{
                        Name = "Uppgift 201",
                        Description = faker.Company.Bs(),
                        Start = new DateTime(2022, 10, 15, 09, 0,0 ),
                        End = new DateTime(2022, 10, 30, 18, 0,0 ),
                        ActivityType = ActivityType.Lecture
                        }
                      }


             }
                    }
                 } ,
             new Course
             {
                 Name = "Programmering 102",
                 Description = faker.Company.Bs(),
                 Start = new DateTime(2022, 12, 16),
                 End = new DateTime(2023, 01, 15),
                 Modules = new List<Module>()

                 {
                     new Module
                     {
                         Name = "Modul 2",
                         Description = faker.Company.Bs(),
                         Start = new DateTime(2022, 10, 16),
                         End = new DateTime(2022, 12, 15),
                         //CourseId = course.Id
                         ModuleActivities = new List<ModuleActivity>() {
                             new ModuleActivity {
                                 Name = "Föreläsning 201",
                                 Description = faker.Company.Bs(),
                                 Start = new DateTime(2022, 10, 15, 09, 0, 0),
                                 End = new DateTime(2022, 10, 15, 11, 0, 0),
                                 ActivityType = ActivityType.Lecture
                             },
                             new ModuleActivity {
                                 Name = "Uppgift 201",
                                 Description = faker.Company.Bs(),
                                 Start = new DateTime(2022, 10, 15, 09, 0, 0),
                                 End = new DateTime(2022, 10, 30, 18, 0, 0),
                                 ActivityType = ActivityType.Lecture
                             }


                     }
                 }
                 }

             }
             };



            return courses;


        }
        // Seedar moduler
        //private static ICollection<Module> GetModules()
        //{
        //    var faker = new Faker("sv");

        //    var modules = new List<Module>()

        //    {
        //       new Module
        //        {
        //            Name = "Modul 1",
        //            Description = faker.Company.Bs(),
        //            Start =new DateTime(2022, 09, 01 ),
        //            End = new DateTime(2022, 10, 15),
        //            //CourseId = course.Id
        //           ModuleActivities = new List<ModuleActivity>(){
        //            new ModuleActivity{
        //                Name = "Föreläsning 101",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 19, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 19, 11, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //           },

        //            new ModuleActivity{
        //                Name = "Föreläsning 102",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 20, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 20, 11, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //            },
        //            new ModuleActivity{
        //                Name = "Föreläsning 103",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 21, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 21, 11, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //           },
        //            new ModuleActivity{
        //                Name = "Föreläsning 104",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 22, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 22, 11, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //           },
        //            new ModuleActivity{
        //                Name = "Föreläsning 105",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 23, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 23, 11, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //           },
        //            new ModuleActivity{
        //                Name = "Uppgift 100",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 01, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 15, 18, 0,0 ),
        //                ActivityType = ActivityType.Task
        //            },
        //           new ModuleActivity{
        //                Name = "Uppgift 101",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 01, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 21, 18, 0,0 ),
        //                ActivityType = ActivityType.Task
        //            },
        //           new ModuleActivity{
        //                Name = "Uppgift 102",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 09, 01, 09, 0,0 ),
        //                End = new DateTime(2022, 09, 23, 18, 0,0 ),
        //                ActivityType = ActivityType.Task
        //            }}
        //       },
        //        new Module
        //        {
        //            Name = "Modul 2",
        //            Description = faker.Company.Bs(),
        //            Start =new DateTime(2022, 10, 16),
        //            End = new DateTime(2022, 12, 15),
        //            //CourseId = course.Id
        //             ModuleActivities = new List<ModuleActivity>(){
        //            new ModuleActivity{
        //                Name = "Föreläsning 201",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 10, 15, 09, 0,0 ),
        //                End = new DateTime(2022, 10, 15, 11, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //                },
        //             new ModuleActivity{
        //                Name = "Uppgift 201",
        //                Description = faker.Company.Bs(),
        //                Start = new DateTime(2022, 10, 15, 09, 0,0 ),
        //                End = new DateTime(2022, 10, 30, 18, 0,0 ),
        //                ActivityType = ActivityType.Lecture
        //                 }
        //              }

        //        }
        //     };
        //    return modules;
        //}

        // Seedar aktiviteter

    }
}

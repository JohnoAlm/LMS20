using LMS20.Core.Entities;
using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS20.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager <ApplicationUser> userManager;

    

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {

            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Courses");
            }

                var user = await userManager.GetUserAsync(User);
                var courseId = user.CourseId;
                var currentModule = db.Courses.SelectMany(c => c.Modules) //Alla Moduler där startdatumet passerat och där slutdatumet inte är passerat för detta kursid
                                             .Where(m => m.Start <= DateTime.Now && m.End >= DateTime.Now)
                                             .FirstOrDefault(c => c.Id == courseId);
                                      
            var course = db.Courses.Include(c => c.Modules) //ta alla kurser och haka på deras moduler => kursen med alla moduler och aktiviteter

                    .ThenInclude(m => m.ModuleActivities) //Haka sedan på varje moduls aktiviteter
                    .FirstOrDefault(c => c.Id == courseId); //jämnför alla kursers id med vårt id
                                                            //===> course innehållervår kurs med dess 
                                                            // muduler och deras aktiviteter
            if (course == null) throw new ArgumentException("Något är fel");

            var myAllAktivities = course.Modules.SelectMany(m => m.ModuleActivities); //ALLA kursens aktiviteter I EN LISTA
                                                                                         
            var myModuleTasks = myAllAktivities.Where(a => a.ActivityType != ActivityType.Lecture ) //Alla Uppgifter(Tasks) inom denna modul : Uppgifter som kan bli försenade
                                               .Where(a => a.Start >= currentModule.Start && currentModule.End > a.End).ToList();
           
            var myModuleActivities = myAllAktivities
                                               .Where(a => a.Start >= currentModule.Start && currentModule.End > a.End);
           
            
            //Is delayed?                                  
            for (int i = 0; i < myModuleTasks.Count(); i++)
            {
                if (myModuleTasks[i].End < DateTime.Now)
                {
                    myModuleTasks[i].ActivityType = ActivityType.Delayed;
                    db.SaveChanges();
                }
            }

            //Denna vecka        
            DateTime myMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek).Date; //Denna måndags datum 

            var thisWeeksactivities = myModuleActivities.Where(a => a.End.Date >= myMonday && a.End.Date <= myMonday.AddDays(6))
                                                        .OrderBy(a => a.End).ToList();

            var res = thisWeeksactivities.GroupBy(a => a.End.ToShortDateString())
                .Select(g => new MyWeek
                {
                    Date = g.Key,
                    Activities = g.ToList()
                });
            var today = thisWeeksactivities.Where(a => a.End.Date == DateTime.Now.Date)
                                            .OrderBy(a => a.End);   
         
            var dashInfo = new IndexViewModel //skapa en ny IndexViewModel som ska populeras
            {
                    Id = course.Id,
                    CourseName = course?.Name,
                    MyTasks = myModuleTasks,
                    MyWeek= thisWeeksactivities,
                    MyWeek2 = res,
                    Today = today,
                    CourseId = courseId,
                    UserId  = user.Id
          

            };
              
            return View(dashInfo);
        }

        public async Task<IActionResult> Participants(int? id)
        {
            var course = await db.Courses.FirstOrDefaultAsync(m => m.Id == id);

            var viewModel = new ParticipantsViewModel
            {
                Id = course.Id,
                Name = course.Name,
                ApplicationUsers = await db.Users.Where(u => u.CourseId == id).ToListAsync()

            };

            return View(viewModel);
        }

        public IActionResult Modules()
        {
            return View();
        }


        //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
    }
}


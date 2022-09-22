using Bogus.DataSets;
using LMS20.Core.Entities;
using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using LMS20.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS20.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager <ApplicationUser> userManager;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{

        //    _logger = logger;
        //}

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.userManager = userManager;

        }

        [Authorize(Roles ="Student")]
        public async Task<IActionResult> Index()
        {

                var user = await userManager.GetUserAsync(User);
                var courseId = user.CourseId;
                var currentModule = db.Courses.SelectMany(c => c.Modules)
                                             .Where(m => m.Start < DateTime.Now && m.End > DateTime.Now)
                                             .FirstOrDefault(c => c.Id == courseId);
                                      

           // if (currentModule == null) throw new ArgumentException("Inga moduler");


            var course = db.Courses.Include(c => c.Modules) //ta alla kurser och haka på deras moduler

                    .ThenInclude(m => m.ModuleActivities) //Haka sedan på varje moduls aktiviteter
                    .FirstOrDefault(c => c.Id == courseId); //jämnför alla kursers id med vårt id
                                                            //===> course innehållervår kurs med dess 
                                                            // muduler och deras aktiviteter
            if (course == null) throw new ArgumentException("Något är fel");

            var myAllAktivities = course.Modules.SelectMany(m => m.ModuleActivities); //ALLA aktiviteter I EN LISTA
                                                                                          // var res =  myModuleAktivities.Where(m => (m.ModuleActivities.Where(x => x.StartDateTime == DateTime.Now)).ToList().Count > 0);
            var myModuleTasks = myAllAktivities.Where(a => a.ActivityType != ActivityType.Lecture )
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
        
            DateTime myMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek).Date;


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
                    CourseName = course?.Name,
                    MyTasks = myModuleTasks,
                    MyWeek= thisWeeksactivities,
                    MyWeek2 = res,
                    Today = today
          

            };
               
               



                return View(dashInfo);
            }

        // db.ModuleActivity.Where()
        //var activities = new List<ModuleActivity>();
        //var todaysaktivities = new List<ModuleActivity>();
        //var activityNames = new List<string>();

        //foreach (var module in course.Modules)
        //{
        //    foreach (var activity in module.ModuleActivities)
        //    {
        //        Check if activity meets criteria
        //         activities.Add(activity);
        //    }
        //}

        //foreach (var activity in activities)
        //{

        //    if (activity.StartDateTime > DateTime.Now && DateTime.Now < activity.EndDateTime)
        //    {
        //        todaysaktivities.Add(activity);
        //        activityNames.Add(activity.Name);
        //    }
        //}
        //    { TodaysActivity = course.Modules.First().ModuleActivities.First() };

        //obs här behöcver vi veta vilken kurse den inloggade går påom det äre en elev.
        //Sätt det värdet på viewmodel
        //2. Transformera tilll ViewModel
        //3. Returnera viewmodel





        public IActionResult Participants()
        {
            return View();
        }
        public IActionResult Modules()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


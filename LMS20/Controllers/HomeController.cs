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
                var currentModule = db.Course.SelectMany(c => c.Modules)
                                             .Where(m => m.StartDateTime < DateTime.Now && m.EndDateTime > DateTime.Now)
                                             .FirstOrDefault(c => c.Id == courseId);
                                      

           // if (currentModule == null) throw new ArgumentException("Inga moduler");


            var course = db.Course.Include(c => c.Modules) //ta alla kurser och haka på deras moduler

                    .ThenInclude(m => m.ModuleActivities) //Haka sedan på varje moduls aktiviteter
                    .FirstOrDefault(c => c.Id == courseId); //jämnför alla kursers id med vårt id
                                                            //===> course innehållervår kurs med dess 
                                                            // muduler och deras aktiviteter
            if (course == null) throw new ArgumentException("Något är fel");

                var myAllAktivities = course.Modules.SelectMany(m => m.ModuleActivities); //ALLA aktiviteter I EN LISTA
                                                                                          // var res =  myModuleAktivities.Where(m => (m.ModuleActivities.Where(x => x.StartDateTime == DateTime.Now)).ToList().Count > 0);
            var myModuleTasks = myAllAktivities.Where(a => a.ActivityType != ActivityType.Lecture )
                                               //.Where(a => a.ActivityType == ActivityType.Delayed)
                                               .Where(a => a.StartDateTime >= currentModule.StartDateTime && currentModule.EndDateTime > a.EndDateTime).ToList();
            var myModuleActivities = myAllAktivities
                                               .Where(a => a.StartDateTime >= currentModule.StartDateTime && currentModule.EndDateTime > a.EndDateTime);
            //Is delayed?                                  


            for (int i = 0; i < myModuleTasks.Count(); i++)
            {

                if (myModuleTasks[i].EndDateTime < DateTime.Now)
                {
                    myModuleTasks[i].ActivityType = ActivityType.Delayed;
                    db.SaveChanges();
                }

            }

            //Denna vecka
            //1.  Hitta datumen för denna vecka
            DateTime myMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek).Date;


            var thisWeeksactivities = myModuleActivities.Where(a => a.EndDateTime.Date >= myMonday && a.EndDateTime.Date <= myMonday.AddDays(6))
                                                        .OrderBy(a => a.EndDateTime).ToList();



            var res = thisWeeksactivities.GroupBy(a => a.EndDateTime.ToShortDateString())
                .Select(g => new MyWeek
                {
                    Date = g.Key,
                    Activities = g.ToList()
                });
            var today = thisWeeksactivities.Where(a => a.EndDateTime.Date == DateTime.Now.Date)
                                            .OrderBy(a => a.EndDateTime);   

            //2. for (myM[i] ; i <=6,
            //for (int i = 0; i < 7 ; i++)
            //{
            //    for (int j = 0; j < myAllAktivities.Count(); j++)
            //    {
            //        if (myMonday.AddDays[i] == myAllAktivities[j].)
            //    }

            //}
            //if akt[i]. date == myM[i]
            // skriv ut myM[i] => flagga 
            //gör en il med infon i akt[i]
            //


            var dashInfo = new IndexViewModel //skapa en ny IndexViewModel som ska populeras
            {
                    CourseName = course?.Name,
                    MyTasks = myModuleTasks,
                    MyWeek= thisWeeksactivities,
                    MyWeek2 = res,
                    Today = today
                //Activities = todaysaktivities,
                //ActivityNames = activityNames,

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


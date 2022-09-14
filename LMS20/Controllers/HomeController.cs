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

        //[Authorize]
        public IActionResult Index()
        {
            //Är vår user en student???
            if (!User.IsInRole("Student"))
            {
                return View(nameof(Modules));//Bör vara lärare men för nu....
            }
            else
            {
                //Rätt kursid för den kurs vår användare går
                var courseId = db.Users.First(u => u.Id == userManager.GetUserId(User))
                    .CourseId;

                
                //
                var course = db.Course.Include(c => c.Modules) //ta alla kurser och haka på deras moduler

                    .ThenInclude(m => m.ModuleActivities) //Haka sedan på varje moduls aktiviteter
                    .FirstOrDefault(c => c.Id == courseId); //jämnför alla kursers id med vårt id
                                                            //===> course innehållervår kurs med dess 
                                                            // muduler och deras aktiviteter


                var dashInfo = new IndexViewModel //skapa en ny IndexViewModel som ska populeras
                {
                    CourseName = course.Name
                };
               
                //    { TodaysActivity = course.Modules.First().ModuleActivities.First() };



                return View(dashInfo);
            }

            //obs här behöcver vi veta vilken kurse den inloggade går påom det äre en elev.
            //Sätt det värdet på viewmodel
            //2. Transformera tilll ViewModel
            //3. Returnera viewmodel



        }

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
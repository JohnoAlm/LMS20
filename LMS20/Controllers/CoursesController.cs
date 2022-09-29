using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMS20.Core.Entities;
using LMS20.Data.Data;
using AutoMapper;
using LMS20.Data.Repositories;
using LMS20.Web.Models;
using LMS20.Core.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using LMS20.Core.Types;

namespace LMS20.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly UnitOfWork uow;
        private readonly UserManager<ApplicationUser> userManager;

        public CoursesController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.mapper = mapper;
            uow = new UnitOfWork(db);
            this.userManager = userManager;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await uow.CourseRepository.GetAllCoursesAsync();
            var coursesViewList = new List<CoursePartialViewModel>();
            var coursesView = new CoursesViewModel();

            if(ModelState.IsValid)
            {
                CoursePartialViewModel viewModel;
                foreach(var course in courses)
                {
                    TimeSpan duration = course.Duration;
                    TimeSpan cLeft = course.End - DateTime.Now;
                    double dProg = (1 - (cLeft / duration)) * 100;
                    int progress = (int)Math.Round(dProg);

                    Status cStatus = 0;
                    if (course.Start > DateTime.Now) cStatus = Status.Comming;
                    if (course.Start < DateTime.Now && course.End > DateTime.Now) cStatus = Status.Current;
                    if (course.End < DateTime.Now) cStatus = Status.Completed;

                    viewModel = new CoursePartialViewModel
                    {
                        Id = course.Id,
                        Name = course.Name,
                        Start = course.Start,
                        End = course.End,
                        CourseStatus = cStatus,
                        Progress = progress,
                        NrOfParticipants = course.ApplicationUsers.Count 
                    };
                    coursesViewList.Add(viewModel);
                }
            }

            coursesView.courses = coursesViewList;

            return View(coursesView);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Courses == null) return NotFound();

            var course = await db.Courses.FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) return NotFound();

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            // return PartialView("CreatePartial")
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCoursePartialViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var course = mapper.Map<Course>(viewModel);

                await uow.CourseRepository.AddCourseAsync(course);
                await uow.CompleteAsync();

                var partial = mapper.Map<CoursePartialViewModel>(course);

                return PartialView("CoursePartial", partial);
            }

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("CreateCoursePartial", viewModel);
        }

        public async Task<JsonResult> ValidateCourseStart(DateTime start)
        {
            if(start < DateTime.Now) return Json("Tiden har redan passerat");

            return Json(true);
        }

        public async Task<JsonResult> ValidateCourseEnd(DateTime End, DateTime Start)
        {
            if(End <= Start) return Json("Sluttiden får inte vara före starttiden");

            return Json(true);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Courses == null)
            {
                return NotFound();
            }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ConfirmDeletePartialViewModel viewModel)
        {
            int id = viewModel.Id;

            await uow.CourseRepository.RemoveCourseAsync(id);
            await uow.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || db.Courses == null) return NotFound();

            var course = await db.Courses.FindAsync(id);
            if(course == null) return NotFound();

            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDateTime,Duration")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(course);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Courses == null)
            {
                return NotFound();
            }

            var course = await db.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            var course = await db.Courses.FindAsync(id);
            if (course != null)
            {
                db.Courses.Remove(course);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (db.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
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
            ViewData["CourseId"] = course.Id;

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}



        public async Task<IActionResult> RegisterUser(int? id)
        {
            var course = await db.Courses.FirstOrDefaultAsync(m => m.Id == id);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegistrationViewModel registrationViewModel)
        {
            registrationViewModel.RegistrationInValid = "true";

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registrationViewModel.Email,
                    Email = registrationViewModel.Email,
                    FirstName = registrationViewModel.FirstName,
                    LastName = registrationViewModel.LastName,
                    CourseId = registrationViewModel.CourseId
                };

                var result = await userManager.CreateAsync(user, registrationViewModel.Password);
                await userManager.AddToRoleAsync(user, "Student");
               
                if (result.Succeeded)
                {
                    registrationViewModel.RegistrationInValid = "";

                    return RedirectToAction(nameof(Participants), new { id = registrationViewModel.CourseId });
                }

                ModelState.AddModelError("", "Registreringsförsök misslyckades");
            }

            return RedirectToAction(nameof(Participants));
        }

        // EditUser GET metod
        public async Task<IActionResult> EditUser(string? id)
        {
            if (id == null || db.Users == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CourseId = user.CourseId,
            };
            ViewData["CourseId"] = user.CourseId;
            return View(editUserViewModel);
        }

        // EditUser POST metod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel editUserViewModel)
        {
            var user = await userManager.FindByIdAsync(editUserViewModel.Id);

            
                if (ModelState.IsValid)
                {
                    user.UserName = editUserViewModel.Email;
                    user.Email = editUserViewModel.Email;
                    user.FirstName = editUserViewModel.FirstName;
                    user.LastName = editUserViewModel.LastName;

                    await userManager.UpdateAsync(user);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Participants), new { id = editUserViewModel.CourseId });
                }

            return RedirectToAction(nameof(Participants));
        }

        private bool UserExists(string id)
        {
            return (db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult Modules(int? id)
             
        {
            var course = db.Courses.FirstOrDefault(m => m.Id == id);


            var modulesModel = new ModulesViewModel
            {
                Course = course,
                CourseName = course.Name,
                Description = course.Description,
            };

            ViewData["CourseId"] = course.Id;

            return View(modulesModel);
        }


       
    }
}

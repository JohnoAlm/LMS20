using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS20.Core.Entities;
using LMS20.Data.Data;
using AutoMapper;
using LMS20.Data.Repositories;
using LMS20.Web.Models;
using LMS20.Core.ViewModels;

namespace LMS20.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly UnitOfWork uow;

        public CoursesController(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            this.mapper = mapper;
            uow = new UnitOfWork(db);
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await uow.CourseRepository.GetAllCoursesAsync();
            var coursesView = new List<CoursesViewModel>();

            if(ModelState.IsValid)
            {
                CoursesViewModel viewModel;
                foreach(var course in courses)
                {
                    var duration = course.Duration;
                    var prog = DateTime.Now - course.StartDateTime;
                    double progress = (prog / duration) *100;

                    viewModel = new CoursesViewModel
                    {
                        Id = course.Id,
                        Name = course.Name,
                        StartDateTime = course.StartDateTime,
                        Duration = course.Duration,
                        Progress = 68 /*progress*/,
                        NrOfParticipants = 15 /*course.ApplicationUsers.Count*/
                    };
                    coursesView.Add(viewModel);
                }
            }

            return View(coursesView);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDateTime,Duration")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Courses == null)
            {
                return NotFound();
            }

            var course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
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
                ApplicationUsers = course.ApplicationUsers,

            };

            return View(viewModel);
        }
    }
}

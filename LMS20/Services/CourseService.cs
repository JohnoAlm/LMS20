using Bogus.DataSets;
using lms20.web.services;
using LMS20.Core.Entities;
using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace LMS20.Web.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        public CourseService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.userManager = userManager;
        }
      

        public  async Task<string> getCourseName(ClaimsPrincipal User)
        {
            var userId = userManager.GetUserId(User);
            var user =  await db.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var courseName = user.Course.Name;
           

            return courseName;
        }

      
        public async Task<string> getCourseId(ClaimsPrincipal User)
        {
            // var courseName = getCourseName(User);
            // var course = await db.Courses
            //                 .FirstOrDefaultAsync(c => c.Name.Equals(courseName));
            //string courseId = course.Id.ToString();

            var userId = userManager.GetUserId(User);
            var user = await db.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var courseId = user.Course.Id.ToString();

            return courseId;
        }
    }
}

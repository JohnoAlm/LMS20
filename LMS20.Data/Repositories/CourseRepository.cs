using LMS20.Core.Entities;
using LMS20.Core.Repositories;
using LMS20.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    /*: ICourseRepository*/
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await db.Courses.Include(p => p.ApplicationUsers).ToListAsync();
        }

        public async Task AddCourseAsync(Course course)
        {
            await db.Courses.AddAsync(course);
        }

    }
}

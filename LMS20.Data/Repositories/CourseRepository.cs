using LMS20.Core.Entities;
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
    {
        private readonly ApplicationDbContext db;
        private ApplicationDbContext db1;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await db.Courses.ToListAsync();
        }
    }
}

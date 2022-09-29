using LMS20.Core.Entities;
using LMS20.Core.Repositories;
using LMS20.Data.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task RemoveCourseAsync(int id)
        {
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if(course != null) db.Courses.Remove(course);
        }
    }
}

using LMS20.Core.Entities;

namespace LMS20.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();        
        Task AddCourseAsync(Course course);
        Task RemoveCourseAsync(int id);

    }
}
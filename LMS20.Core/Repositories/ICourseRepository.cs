using LMS20.Core.Entities;

namespace LMS20.Core.Repositories
{
    public interface ICourseRepository
    {
        Task AddCourseAsync(Course course);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
    }
}
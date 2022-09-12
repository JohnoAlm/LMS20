using LMS20.Core.Entities;

namespace LMS20.Data.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
    }
}
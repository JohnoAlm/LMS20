namespace LMS20.Core.Repositories
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
    }
}
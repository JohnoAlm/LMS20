namespace LMS20.Data.Repositories
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
    }
}
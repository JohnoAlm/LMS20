namespace LMS20.Data.Repositories
{
    public interface IUnitOfWork
    {
        CourseRepository CourseRepository { get; }
    }
}
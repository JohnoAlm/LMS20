using LMS20.Core.Repositories;
using LMS20.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public ICourseRepository CourseRepository { get; }

        public UnitOfWork(ApplicationDbContext db)
        {
            CourseRepository = new CourseRepository(db);
            this.db = db;
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

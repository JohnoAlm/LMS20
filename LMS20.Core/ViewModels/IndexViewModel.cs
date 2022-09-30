using LMS20.Core.Entities;

namespace LMS20.Core.ViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }

        public string ClassStart { get; set; } = "09.00-12.00";      
        public IEnumerable<ModuleActivity>? MyTasks { get; set; }
        public List<ModuleActivity>? MyWeek { get; set; }
        public IEnumerable<MyWeek> MyWeek2 { get;  set; }
        public IOrderedEnumerable<ModuleActivity> Today { get; set; }
        public int? CourseId { get; set; }
        public string? UserId { get; set; }

   

    }
}

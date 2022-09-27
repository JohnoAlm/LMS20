using LMS20.Core.Entities;

namespace LMS20.Core.ViewModels
{
    public class IndexViewModel
    {
        public string CourseName { get; set; }

       //public List<ModuleActivity> Activities { get; set; } = new List<ModuleActivity>();
        //public List<string> ActivityNames { get; set; } = new List<string>();
        //public string Description { get; set; } = string.Empty;
        //public DateTime StartDateTime { get; set; }
        //public TimeSpan Duration { get; set; }

        public string ClassStart { get; set; } = "09.00-12.00";
       // public IEnumerable <ModuleActivity> TodaysActivity { get; set; }
      
        public IEnumerable<ModuleActivity>? MyTasks { get; set; }
        public List<ModuleActivity>? MyWeek { get; set; }
        public IEnumerable<MyWeek> MyWeek2 { get;  set; }
        public IOrderedEnumerable<ModuleActivity> Today { get; set; }
        public int? CourseId { get; set; }
        public string? UserId { get; set; }

        // public IEnumerable<IndexViewModel> Modules { get; set; }

    }
}

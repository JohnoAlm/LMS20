using LMS20.Core.Entities;

namespace LMS20.Core.ViewModels
{
    public class MyWeek
    {
        public string Date { get; set; }
        public List<ModuleActivity> Activities { get; set; }
    }
}
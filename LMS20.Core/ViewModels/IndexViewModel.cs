﻿using LMS20.Core.Entities;

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
       public string taskTime { get; set; } = "Senast ";
        public IEnumerable<ModuleActivity>? MyTasks { get; set; }

        // public IEnumerable<IndexViewModel> Modules { get; set; }

    }
}

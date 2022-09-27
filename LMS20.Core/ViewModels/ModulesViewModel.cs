using LMS20.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
   public class ModulesViewModel
    {

        public int? CourseId { get; set; }
        public IEnumerable<Module>? CourseModules { get; set; }
        public Course Course { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
    }
}

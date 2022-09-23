using LMS20.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
    public class CoursesViewModel
    {
        public CreateCoursePartialViewModel createCourse { get; set; }
        public IEnumerable<CoursePartialViewModel> courses { get; set; } = new List<CoursePartialViewModel>();
        public string Name { get; set; }

    }
}

using LMS20.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
    public class CoursePartialViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Status CourseStatus { get; set; }
        public int NrOfParticipants { get; set; }
        public int Progress { get; set; }
    }
}

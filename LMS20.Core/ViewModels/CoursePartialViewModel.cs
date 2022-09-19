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
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return (StartDateTime + Duration); } }
        public Status CourseStatus { get; set; }
        public int NrOfParticipants { get; set; }
        public int Progress { get; set; }
    }
}

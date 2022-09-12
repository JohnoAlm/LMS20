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
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; } = "KURS 101";

        [Display(Name = "Starttid")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "Längd")]
        public TimeSpan Duration { get; set; }

        [Display(Name = "Antal deltagare")]
        public int NrOfParticipants { get; set; }
    }
}

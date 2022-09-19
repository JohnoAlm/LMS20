using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
    public class CreateCoursePartialViewModel
    {
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Beskrivning")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Starttid")]
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; } = DateTime.Now + TimeSpan.FromDays(1);

        [Required]
        [Display(Name = "Sluttid")]
        public DateTime EndDateTime { get; set; } 
    }
}

using LMS20.Core.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


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
        //[Remote("ValidateCourseStart", "Courses")]
        [Display(Name = "Starttid")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; } = DateTime.Now + TimeSpan.FromDays(1);

        [Required]
        //[Remote("ValidateCourseEnd", "Courses", AdditionalFields = "Start")]
        [ValidateCourseDate(ErrorMessage = "Sluttid måste vara senare än starttid")]
        [Display(Name = "Sluttid")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; } = DateTime.Now + TimeSpan.FromDays(7);
    }
}
using LMS20.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
#nullable disable
    public class RegistrationViewModel
    {
        public string UserName { get { return Email; }  }

        [Required]
        [EmailAddress]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        public Course Course { get; set; }

        public int? CourseId { get; set; }

        public bool AcceptUserAgreement { get; set; }

        public string RegistrationInValid { get; set; }
    }
}

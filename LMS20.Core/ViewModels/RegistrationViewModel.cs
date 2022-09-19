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
        [Required]
        [EmailAddress]
        [Display(Name = "Användarnamn")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        public bool AcceptUserAgreement { get; set; }
        public string RegistrationInValid { get; set; }
    }
}

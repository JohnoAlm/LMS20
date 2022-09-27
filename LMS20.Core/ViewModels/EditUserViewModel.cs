using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get { return Email; } }

        [Required]
        [EmailAddress]
        [Display(Name = "Användarnamn")]
        public string Email { get; set; }



        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Nuvarande lösenord")]
        //public string CurrentPassword { get; set; }

        //[Required]
        //[Compare("Password")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Nytt lösenord")]
        //public string NewPassword { get; set; }

        //[Required]
        //[Compare("Password")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Bekräfta lösenord")]
        //[Compare("NewPassword", ErrorMessage = "Bekräfta lösenord matchar inte")]
        //public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        public int? CourseId { get; set; }

        
    }
}

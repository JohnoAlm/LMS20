using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LMS20.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Förnamn")]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        [StringLength(250)]
        public string LastName { get; set; }

        public Course ? Course { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Course Course { get; set; }
        public int? CourseId { get; set; }
    }
}

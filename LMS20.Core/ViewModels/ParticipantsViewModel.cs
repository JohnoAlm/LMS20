using LMS20.Core.Entities;

namespace LMS20.Core.ViewModels
{
#nullable disable
    public class ParticipantsViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
        public RegistrationViewModel RegistrationViewModel { get; set; }
        public EditUserViewModel EditUserViewModel { get; set; }

        public Course Course { get; set; }
    }
}

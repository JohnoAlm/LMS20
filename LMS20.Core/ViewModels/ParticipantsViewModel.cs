using LMS20.Core.Entities;

namespace LMS20.Core.ViewModels
{
#nullable disable
    public class ParticipantsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
        public Course Course { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LMS20.Core.Entities
{
#nullable disable
    public class ModuleActivity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Module Module { get; set; }
        public int ModuleId { get; set; }
        public ActivityType ActivityType { get; set; }
    }
  
}

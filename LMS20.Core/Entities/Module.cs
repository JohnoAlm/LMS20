namespace LMS20.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public ICollection<ModuleActivity> ModuleActivities { get; set; } = new List<ModuleActivity>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Course Course { get; set; } = new();

        public int CourseId { get; set; }
    }
}

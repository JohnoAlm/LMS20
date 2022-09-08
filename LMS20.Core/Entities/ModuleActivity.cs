namespace LMS20.Core.Entities
{
    public class ModuleActivity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();

        public int ModuleId { get; set; }
    }
}

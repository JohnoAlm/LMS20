namespace LMS20.Core.Dtos
{
    public class CourseDto
    {
        
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
    
        public TimeSpan Duration { get; set; }

      
    }
}

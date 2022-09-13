namespace LMS20.Core.Dtos
{
    public class ModuleActivityDto
    {
   
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public String ClassStart { get; set; } = "09.00-11.00";
  
    }
}

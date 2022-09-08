
using System.Diagnostics.SymbolStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDateTime { get; set; }

        // public IFormFile FormFile { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ModuleActivityId { get; set; }
    }
}

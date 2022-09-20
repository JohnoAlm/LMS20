using LMS20.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
    public class TestCreateModuleViewModel
    {
        public int Id { get; set; }             // Kursens PK

        public DateTime Start { get; set; }     // Föreslagen modul starttime
        
        public DateTime End { get; set; }       // Föreslagen modul endtime

        public ICollection<Module> Modules { get; set; } = new List<Module>();

    }
}

using LMS20.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.ViewModels
{
    public class ParticipantsViewModel
    {
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}

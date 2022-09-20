using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.Services
{
    public interface IValidateDateService
    {
        Task<string> ValidateCourseDate(DateTime Start, DateTime End);
        Task<string> ValidateModuleDate(DateTime Start, DateTime End, int CourseId);
        Task<string> ValidateActivityDate(DateTime Start, DateTime End, int ModuleId);

    }
}

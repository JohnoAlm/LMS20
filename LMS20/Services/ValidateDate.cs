using LMS20.Core.Services;
using LMS20.Data.Data;

namespace LMS20.Web.Services
{
    public class ValidateDate : IValidateDateService
    {
        private readonly ApplicationDbContext db;

        //få in dbcontext eller uow via konstruktor
        public ValidateDate(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<string> ValidateCourseDate(DateTime start, DateTime end)
        {
            if(start < DateTime.Now) return "Tiden har redan passerat";
            if(end <= start) return "Sluttiden får inte vara före starttiden";

            return "Ok";
        }

        public async Task<string> ValidateModuleDate(DateTime start, DateTime end, int courseId)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course is null) return "Ingen kurs funnen";

            // Startar före eller slutar efter kursen
            if (start < course.Start || end > course.End) return "Tiden måste ligga inom kursen";

            foreach(var module in course.Modules)
            {
                if (start < module.Start && end > module.End)                   // Omsluter helt en existerande modul
                    return "Tiden överlappar en existerande modul"; 
                
                if (start > module.Start && end < module.End)                   // Omsluts helt av en existerande modul
                    return "Tiden upptas av en existerande modul";

                if (start < module.Start && end > module.Start)                 // Överlappar starten på en existerande modul
                    return "Tiden överlappar starten på en existerande modul";

                if (start < module.End && end > module.End)                     // Överlappar slutet på en existerande modul
                    return "Tiden överlappar slutet på en existerande modul";
            }

            return "Ok";
        }

        public async Task<string> ValidateActivityDate(DateTime start, DateTime end, int moduleId)
        {
            var module = db.Modules.FirstOrDefault(m => m.Id == moduleId);
            if (module is null) return "Ingen modul funnen";

            // Startar före eller slutar efter modulen
            if (start < module.Start || end > module.End) return "Tiden måste ligga inom modulen";

            foreach (var activity in module.ModuleActivities)
            {
                if (start < activity.Start && end > activity.End)               // Omsluter helt en existerande modul
                    return "Tiden överlappar en existerande aktivitet";

                if (start > activity.Start && end < activity.End)               // Omsluts helt av en existerande modul
                    return "Tiden upptas av en existerande aktivitet";

                if (start < activity.Start && end > activity.Start)             // Överlappar starten på en existerande modul
                    return "Tiden överlappar starten på en existerande aktivitet";

                if (start < activity.End && end > activity.End)                 // Överlappar slutet på en existerande modul
                    return "Tiden överlappar slutet på en existerande aktivitet";
            }

            return "Ok";
        }

    }
}

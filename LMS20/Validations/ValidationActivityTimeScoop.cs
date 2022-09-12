using LMS20.Core.Entities;
using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LMS20.Web.Validations
{
    public class ValidationActivityTimeScoop : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is DateTime input)
            {
                var vm = validationContext.ObjectInstance as TestCreateActivityViewModel;
                var db = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                if(vm is null || db is null) return new ValidationResult(ErrorMessage);

                var module = db.Modules.FirstOrDefault(m => m.Id == vm.Id);

                var startTime = vm.ActivityStartTime;
                var endTime = vm.ActivityStartTime + vm.ActivityDuration;       // input ?

                if (startTime < DateTime.Now) return new ValidationResult(ErrorMessage);

                // Startar före eller slutar efter modulen
                if(startTime < module.StartDateTime ||
                    endTime > (module.StartDateTime + module.Duration)) 
                    return new ValidationResult(ErrorMessage);

                DateTime activityStartTime, activityEndTime;
                foreach(var activity in module.ModuleActivities)
                {
                    activityStartTime = activity.StartDateTime;
                    activityEndTime = activity.StartDateTime + activity.Duration;

                    // Omsluter helt en existerande aktivitet
                    if (startTime < activityStartTime &&                 
                        endTime > activityEndTime) return new ValidationResult(ErrorMessage);

                    // Omsluts helt av en existerande aktivitet
                    if (startTime > activityStartTime &&                 
                        endTime < activityEndTime) return new ValidationResult(ErrorMessage);

                    // Överlappar starten på en existerande aktivitet
                    if (startTime < activityStartTime &&                 
                        (endTime > activityStartTime)) return new ValidationResult(ErrorMessage);

                    // Överlappar slutet på en existerande aktivitet
                    if (startTime < activityEndTime &&                   
                        endTime > activityEndTime) return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

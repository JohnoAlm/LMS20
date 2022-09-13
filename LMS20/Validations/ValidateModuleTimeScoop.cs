using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace LMS20.Web.Validations
{
    public class ValidateModuleTimeScoop : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is DateTime input)
            {
                var vm = validationContext.ObjectInstance as TestCreateModuleViewModel;
                var db = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                if(vm is null || db is null) return new ValidationResult(ErrorMessage);

                var startTime = vm.ModuleStartTime;
                var endTime = vm.ModuleStartTime + vm.ModuleDuration;       // input ?

                if (startTime < DateTime.Now) return new ValidationResult(ErrorMessage);

                var course = db.Course.FirstOrDefault(m => m.Id == vm.Id);

                // Startar före eller slutar efter modulen
                if(startTime < course.StartDateTime ||
                    endTime > (course.StartDateTime + course.Duration))
                    return new ValidationResult(ErrorMessage);

                DateTime moduleStartTime, moduleEndTime;
                foreach(var module in course.Modules)
                {
                    moduleStartTime = module.StartDateTime;
                    moduleEndTime = module.StartDateTime + module.Duration;

                    // Omsluter helt en existerande aktivitet
                    if (startTime < moduleStartTime &&
                        endTime > moduleEndTime) return new ValidationResult(ErrorMessage);

                    // Omsluts helt av en existerande aktivitet
                    if (startTime > moduleStartTime &&
                        endTime < moduleEndTime) return new ValidationResult(ErrorMessage);

                    // Överlappar starten på en existerande aktivitet
                    if (startTime < moduleStartTime &&
                        (endTime > moduleStartTime)) return new ValidationResult(ErrorMessage);

                    // Överlappar slutet på en existerande aktivitet
                    if (startTime < moduleEndTime &&
                        endTime > moduleEndTime) return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

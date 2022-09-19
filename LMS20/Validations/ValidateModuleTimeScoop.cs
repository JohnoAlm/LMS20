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
                var endTime = vm.ModuleEndTime;

                if (startTime < DateTime.Now) return new ValidationResult(ErrorMessage);

                var course = db.Courses.FirstOrDefault(m => m.Id == vm.Id);

                // Startar före eller slutar efter modulen
                if(startTime < course.Start || endTime > course.End)
                    return new ValidationResult(ErrorMessage);

                DateTime moduleStartTime, moduleEndTime;
                foreach(var module in course.Modules)
                {
                    moduleStartTime = module.StartDateTime;
                    moduleEndTime = module.EndDateTime;

                    // Omsluter helt en existerande aktivitet
                    if (startTime < moduleStartTime && endTime > moduleEndTime) 
                        return new ValidationResult(ErrorMessage);

                    // Omsluts helt av en existerande aktivitet
                    if (startTime > moduleStartTime && endTime < moduleEndTime) 
                        return new ValidationResult(ErrorMessage);

                    // Överlappar starten på en existerande aktivitet
                    if (startTime < moduleStartTime && endTime > moduleStartTime) 
                        return new ValidationResult(ErrorMessage);

                    // Överlappar slutet på en existerande aktivitet
                    if (startTime < moduleEndTime && endTime > moduleEndTime) 
                        return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

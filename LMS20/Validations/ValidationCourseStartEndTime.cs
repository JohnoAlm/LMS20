using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace LMS20.Web.Validations
{
    public class ValidationCourseStartEndTime : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is DateTime input)
            {
                var vm = validationContext.ObjectInstance as CreateCoursePartialViewModel;
                var db = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                if(vm is null || db is null) return new ValidationResult(ErrorMessage);
                if(vm.StartDateTime < DateTime.Now) return new ValidationResult(ErrorMessage); 
                if(vm.EndDateTime <= vm.StartDateTime) return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

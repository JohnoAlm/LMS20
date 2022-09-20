using LMS20.Core.Services;
using LMS20.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS20.Core.Validations
{
    public class ValidateCourseDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var vm = validationContext.ObjectInstance as CreateCoursePartialViewModel;
            var validationService = (IValidateDateService)validationContext.GetService(typeof(IValidateDateService));

            var result = validationService.ValidateCourseDate(vm.Start, vm.End).ToString();

            if (result == "Ok") return ValidationResult.Success;

            return new ValidationResult(result);
        }
    }

    public class ValidateModuleDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var vm = validationContext.ObjectInstance as TestCreateModuleViewModel;
            var validationService = (IValidateDateService)validationContext.GetService(typeof(IValidateDateService));

            var result = validationService.ValidateModuleDate(vm.Start, vm.End, vm.Id).ToString();

            if (result == "Ok") return ValidationResult.Success;

            return new ValidationResult(result);
        }
    }

    public class ValidateActivityDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var vm = validationContext.ObjectInstance as TestCreateActivityViewModel;
            var validationService = (IValidateDateService)validationContext.GetService(typeof(IValidateDateService));

            var result = validationService.ValidateActivityDate(vm.Start, vm.End, vm.Id).ToString();

            if (result == "Ok") return ValidationResult.Success;

            return new ValidationResult(result);
        }
    }
}

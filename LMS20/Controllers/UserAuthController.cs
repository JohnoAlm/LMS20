using AutoMapper;
using LMS20.Core.Entities;
using LMS20.Core.ViewModels;
using LMS20.Data.Data;
using LMS20.Web.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS20.Web.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserAuthController(ApplicationDbContext context, 
                                  UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RegisterUser(RegistrationViewModel registrationViewModel)
        //{
        //    registrationViewModel.RegistrationInValid = "true";

        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = registrationViewModel.Email,
        //            Email = registrationViewModel.Email,
        //            FirstName = registrationViewModel.FirstName,
        //            LastName = registrationViewModel.LastName,
        //        };

           

        //        var result = await _userManager.CreateAsync(user, registrationViewModel.Password);
        //        await _userManager.AddToRoleAsync(user, "Student");
        //        await _context.SaveChangesAsync();

        //        if (result.Succeeded)
        //        {
        //            registrationViewModel.RegistrationInValid = "";

        //            await _signInManager.SignInAsync(user, isPersistent: false);

        //            return PartialView("AddStudentPartial", registrationViewModel);
        //        }

        //        ModelState.AddModelError("", "Registreringsförsök misslyckades");
        //    }

        //    return PartialView("AddStudentPartial", registrationViewModel);
        //}



       
    }
}

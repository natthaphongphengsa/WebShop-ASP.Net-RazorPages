using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.ViewModels;

namespace WebbShop.Pages.Account
{
    public class LoginModel : PageModel
    {
        public SignInManager<IdentityUser> SignInManager { get; }
        [BindProperty]
        public Login Model { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public void OnGet()
        {
           
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var SignInresult  = await SignInManager.PasswordSignInAsync(Model.EmailAddress, Model.Password, Model.Rememberme, false);
                if (SignInresult.Succeeded)
                {
                    if(returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToPage("/Index");
                    }
                    return RedirectToPage(returnUrl);
                }
                ModelState.AddModelError("", "Your username or password is incorrect! Try Again");
            }
            return Page();
        }
    }
}

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
        public UserManager<IdentityUser> UserManager { get; }
        [BindProperty]
        public Login Model { get; set; }
        [BindProperty]
        public string LoginMessage { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var SignInresult = await SignInManager.PasswordSignInAsync(Model.EmailAddress, Model.Password, Model.Rememberme, false);

                if (SignInresult.Succeeded)
                {
                    var user = await UserManager.FindByEmailAsync(Model.EmailAddress);
                    var userrole = await UserManager.GetRolesAsync(user);
                    string result = userrole[0].ToString();
                    //if (returnUrl == null || returnUrl == "/")
                    //{
                    //    return RedirectToPage("/Index");
                    //}
                    //return RedirectToPage(returnUrl);

                    if ( result == "Admin")
                    {
                        if (returnUrl == null || returnUrl == "/")
                        {
                            return RedirectToPage("/Admin/AdminPage");
                        }
                        return RedirectToPage(returnUrl);
                    }
                    else
                    {
                        if (returnUrl == null || returnUrl == "/")
                        {
                            return RedirectToPage("/Index");
                        }
                        return RedirectToPage(returnUrl);
                    }
                }
                ModelState.AddModelError("failed", "Your username or password is incorrect! Try Again");

            }
            return Page();
        }
    }
}

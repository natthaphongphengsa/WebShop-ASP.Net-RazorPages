using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebbShop.Pages.Account
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool Rememberme { get; set; }
        public SignInManager<IdentityUser> SignInManager;
        public UserManager<IdentityUser> UserManager;

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
                var SignInresult = await SignInManager.PasswordSignInAsync(EmailAddress, Password, Rememberme, false);

                if (SignInresult.Succeeded)
                {
                    var user = await UserManager.FindByEmailAsync(EmailAddress);
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

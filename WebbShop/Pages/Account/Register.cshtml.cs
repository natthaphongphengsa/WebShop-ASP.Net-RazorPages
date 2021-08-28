using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.Data;
using WebbShop.ViewModels;

namespace WebbShop.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Register Registermodel { get; set; }

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public readonly ApplicationDbContext _dbContext;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var Customer = new IdentityUser()
                {
                    UserName = Registermodel.EmailAddress,
                    Email = Registermodel.EmailAddress,                    
                };
                if (!_dbContext.Users.Any(u => u.UserName == Customer.UserName))
                {
                    var password = new PasswordHasher<IdentityUser>();
                    var hashed = password.HashPassword(Customer, Registermodel.Password);
                    Customer.PasswordHash = hashed;

                    _dbContext.Users.Add(Customer);
                    var UserRole = new IdentityUserRole<string>()
                    {
                        RoleId = _dbContext.Roles.First(c => c.Name == "Customer").Id,
                        UserId = Customer.Id
                    };
                    _dbContext.UserRoles.Add(UserRole);
                }
                var result = await userManager.CreateAsync(Customer, Customer.PasswordHash);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(Customer, false);
                    return RedirectToPage("/Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return Page();
        }
    }
}

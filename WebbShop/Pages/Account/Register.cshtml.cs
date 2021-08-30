using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.Data;

namespace WebbShop.Pages.Account
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Onlynumber is allowed")]
        public string PhoneNumber { get; set; }
        [MaxLength(12)]
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string ConfirmPassword { get; set; }

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public readonly ApplicationDbContext _dbContext;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
                    UserName = EmailAddress,
                    Email = EmailAddress,
                    PhoneNumber = PhoneNumber,
                    PasswordHash = Password
                    
                };
                if (!_dbContext.Users.Any(u => u.UserName == Customer.Email))
                {
                    //var password = new PasswordHasher<IdentityUser>();
                    //var hashed = password.HashPassword(Customer, Registermodel.Password);
                    //Customer.PasswordHash = hashed;

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

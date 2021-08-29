using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Pages.Admin.Management
{
    [Authorize(Roles = "Admin")]
    public class CreateCategoryModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        [BindProperty]
        [Required(ErrorMessage = "Category name is required!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public CreateCategoryModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (!_dbContext.category.Any(c => c.Name == Name))
                {
                    _dbContext.category.Add(new Category() { Name = Name });
                    _dbContext.SaveChanges();
                }

                return RedirectToPage("/Admin/Management/Confirm", new { text = "Your new category is now added to database", id = 2 });
            }
            return Page();
        }
    }
}

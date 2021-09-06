using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Pages.Admin.Management
{
    [Authorize(Roles = "Admin,ProductManager")]
    public class CategoryListModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly INotyfService notyf;
        public CategoryListModel(ApplicationDbContext dbContext, INotyfService notyfService)
        {
            notyf = notyfService;
            _dbContext = dbContext;
        }
        public List<Category> categories { get; set; } = new List<Category>();
        public List<Product> products { get; set; } = new List<Product>();
        public void OnGet()
        {
            categories = _dbContext.category.ToList();
            products = _dbContext.product.ToList();
        }
        public IActionResult OnPost(int id)
        {
            var categories = _dbContext.category.First(c => c.Id == id);
            _dbContext.category.Remove(categories);
            _dbContext.SaveChanges();
            notyf.Success($"Category: {categories.Name} is now Deleted from database", 3);
            return RedirectToPage("/Admin/Management/CategoryList");
        }
    }
}

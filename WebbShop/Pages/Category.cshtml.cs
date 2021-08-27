using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Pages
{
    public class CategoryModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        public CategoryModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Product> products { get; set; } = new List<Product>();
        public List<Category> categories { get; set; } = new List<Category>();
        public string CategoryName { get; set; }

        public void OnGet(int id)
        {

            categories = _dbContext.category.ToList();
            var category = categories.First(c => c.Id == id);
            CategoryName = category.Name;
            products = _dbContext.product.Where(c => c.Category.Id == id).ToList();

        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using WebbShop.Data;
using WebbShop.Models;
using WebbShop.Session;

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
        public List<ImageFile> imageFiles { get; set; } = new List<ImageFile>();
        public string CategoryName { get; set; }

        public void OnGet(int id)
        {
            var count = HttpContext.Session.Get<List<Product>>("CartList");
            if (count != null)
            {

                ViewData["Amount"] = count.Count();

            }
            categories = _dbContext.category.ToList();
            imageFiles = _dbContext.imagefiles.ToList();
            var category = categories.First(c => c.Id == id);
            CategoryName = category.Name;
            products = _dbContext.product.Where(c => c.Category.Id == id).ToList();

        }
    }
}

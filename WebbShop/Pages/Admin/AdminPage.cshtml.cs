using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminPageModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        public AdminPageModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Product> products { get; set; } = new List<Product>();
        public List<ImageFile> imageFiles { get; set; } = new List<ImageFile>();
        public void OnGet()
        {
            products = _dbContext.product.ToList();
            imageFiles = _dbContext.imagefiles.ToList();            
        }
        public IActionResult OnPost(int id)
        {
            var product = _dbContext.product.First(c => c.Id == id);
            var image = _dbContext.imagefiles.First(i => i.product == product);
            _dbContext.imagefiles.Remove(image);
            _dbContext.product.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToPage("/Admin/Management/Confirm", new { text = "Your product is now deleted from database", id = 1 });
        }
    }
}

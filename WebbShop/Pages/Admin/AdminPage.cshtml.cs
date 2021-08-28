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
        public void OnGet()
        {
            products = _dbContext.product.ToList();
        }
        public void OnPost(int id)
        {            
            var product = _dbContext.product.First(c => c.Id == id);
            _dbContext.product.Remove(product);
            _dbContext.SaveChanges();
            OnGet();
        }
    }
}

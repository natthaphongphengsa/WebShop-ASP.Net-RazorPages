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
    public class ProductDetailModel : PageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public Category Category { get; set; }

        public readonly ApplicationDbContext _dbContext;

        public ProductDetailModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet(int id)
        {
            var item = _dbContext.product.FirstOrDefault(c => c.Id == id);
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Image = item.Image;
            Category = _dbContext.product.First(c => c.Id == id).Category;
        }
    }
}

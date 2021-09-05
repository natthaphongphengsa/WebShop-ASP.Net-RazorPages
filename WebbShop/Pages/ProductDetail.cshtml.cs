using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebbShop.Data;
using WebbShop.Models;
using WebbShop.Session;

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
            var count = HttpContext.Session.Get<List<Product>>("CartList");
            if (count != null)
            {

                ViewData["Amount"] = count.Count();

            }
            var item = _dbContext.product.Include(c =>c.Category).ToList().FirstOrDefault(c => c.Id == id);
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Category = item.Category;

            var img = _dbContext.imagefiles.First(c => c.product == item);
            if (img.DataFiles == null)
            {
                Image = img.Filename;
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.DataFiles);
                string ImageUrl = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                Image = ImageUrl;

            }
        }
    }
}

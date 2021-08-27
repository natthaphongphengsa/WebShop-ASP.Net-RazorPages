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

namespace WebbShop.Pages.Admin.Product_Management
{
    [Authorize(Roles = "Admin")]
    public class ProductListModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        public ProductListModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Product> products { get; set; } = new List<Product>();
        public List<Category> categories { get; set; } = new List<Category>();

        public void OnGet()
        {
            categories = _dbContext.category.ToList();
            products = _dbContext.product.ToList();

        }
    }
}

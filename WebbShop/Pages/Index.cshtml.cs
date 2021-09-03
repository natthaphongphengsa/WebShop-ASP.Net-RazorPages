using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebbShop.Data;
using WebbShop.Models;
using WebbShop.Session;

namespace WebbShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public readonly ApplicationDbContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<Product> products { get; set; } = new List<Product>();
        public List<Category> categories { get; set; } = new List<Category>();
        public List<ImageFile> imageFiles { get; set; } = new List<ImageFile>();
        public void OnGet()
        {
            var count = HttpContext.Session.Get<List<Product>>("CartList");
            if (count != null)
            {

                ViewData["Amount"] = count.Count();

            }
            products = _dbContext.product.ToList();
            imageFiles = _dbContext.imagefiles.ToList();
            categories = _dbContext.category.ToList();
        }
    }
}

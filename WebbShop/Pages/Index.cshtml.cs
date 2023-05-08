using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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
            imageFiles = _dbContext.imagefiles.ToList();
            categories = _dbContext.category.ToList();
        }

        public List<Product> products { get; set; } = new List<Product>();
        public List<Category> categories { get; set; } = new List<Category>();
        public List<ImageFile> imageFiles { get; set; } = new List<ImageFile>();
        public List<SelectListItem> FilterList { get; set; } = new List<SelectListItem>();
        public string SeachInput { get; private set; }

        public int selectedFilter;

        public void OnGet()
        {
            var count = HttpContext.Session.Get<List<Product>>("CartList");
            if (count != null)
            {

                ViewData["Amount"] = count.Count();

            }
            //products = _dbContext.product.ToList();

            OnPost(selectedFilter);
        }
        public IActionResult OnPost(int id)
        {
            var count = HttpContext.Session.Get<List<Product>>("CartList");
            if (count != null)
            {

                ViewData["Amount"] = count.Count();

            }
            selectedFilter = id;
            FilterList.Add(new SelectListItem() { Text = "Low price first", Value = "0", });
            FilterList.Add(new SelectListItem() { Text = "High price first", Value = "1", });
            FilterList.Add(new SelectListItem() { Text = "Alphabetical order", Value = "2", });
            switch (id)
            {
                case 0:
                    products = _dbContext.product.OrderBy(c => c.Price).ToList();
                    break;
                case 1:
                    products = _dbContext.product.OrderByDescending(c => c.Price).ToList();
                    break;
                case 2:
                    products = _dbContext.product.OrderBy(c => c.Name).ToList();
                    break;
                default:
                    products = _dbContext.product.ToList();
                    break;
            }
            return Page();
        }
    }
}

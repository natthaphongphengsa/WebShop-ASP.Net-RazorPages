using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbShop.Data;
using WebbShop.Models;
using WebbShop.Session;

namespace WebbShop.Pages
{
    public class SearchProductsModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;

        public List<Product> products { get; set; } = new List<Product>();
        public List<Category> categories { get; set; } = new List<Category>();
        public List<ImageFile> imageFiles { get; set; } = new List<ImageFile>();
        public List<SelectList> selected { get; set; } = new List<SelectList>();
        public List<SelectListItem> FilterList { get; set; } = new List<SelectListItem>();

        public int selectedFilter { get; set; } 
        public string SeachInput { get; set; }

        public SearchProductsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            categories = _dbContext.category.ToList();
            imageFiles = _dbContext.imagefiles.ToList();
        }
        public void OnGet(string searchinput)
        {
            SeachInput = searchinput;
            OnPost(selectedFilter, searchinput);
        }

        public IActionResult OnPost(int id, string search)
        {
            var count = HttpContext.Session.Get<List<Product>>("CartList");
            if (count != null)
            {

                ViewData["Amount"] = count.Count();

            }
            SeachInput = search;
            selectedFilter = id;
            FilterList.Add(new SelectListItem() { Text = "Low price first", Value = "0", });
            FilterList.Add(new SelectListItem() { Text = "High price first", Value = "1", });
            FilterList.Add(new SelectListItem() { Text = "Alphabetical order", Value = "2", });
            switch (id)
            {
                case 0:
                    products = _dbContext.product.OrderBy(c => c.Price).Where(c => c.Name.Contains(search)).ToList();                    
                    break;
                case 1:
                    products = _dbContext.product.OrderByDescending(c => c.Price).Where(c => c.Name.Contains(search)).ToList();
                    break;
                case 2:
                    products = _dbContext.product.OrderBy(c => c.Name).Where(c => c.Name.Contains(search)).ToList();
                    break;
                default:
                    products = _dbContext.product.Where(c => c.Name.Contains(search)).ToList();
                    break;
            }
            return Page();
        }
    }
}

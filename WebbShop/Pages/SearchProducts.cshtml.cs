using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Pages
{
    public class SearchProductsModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;

        public List<Product> products { get; set; } = new List<Product>();
        public List<Category> categories { get; set; } = new List<Category>();
        public List<SelectList> selected { get; set; } = new List<SelectList>();
        public List<SelectListItem> FilterList { get; set; } = new List<SelectListItem>();

        public int selectedFilter { get; set; } 
        public string SeachInput { get; set; }

        public SearchProductsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet(string searchinput)
        {
            SeachInput = searchinput;
            OnPost(selectedFilter, searchinput);
            categories = _dbContext.category.ToList();
        }

        public IActionResult OnPost(int id, string search)
        {
            SeachInput = search;
            selectedFilter = id;
            FilterList.Add(new SelectListItem() { Text = "Billigast först", Value = "0", });
            FilterList.Add(new SelectListItem() { Text = "Dyraste först", Value = "1", });
            FilterList.Add(new SelectListItem() { Text = "Bokstavsordning", Value = "2", });
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

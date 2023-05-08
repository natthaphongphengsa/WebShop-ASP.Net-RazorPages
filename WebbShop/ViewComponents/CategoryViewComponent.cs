using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        public readonly ApplicationDbContext _dbContext;
        public CategoryViewComponent(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            List<Category> categories = new List<Category>();
            categories = _dbContext.category.ToList();
            return View("Category", categories);
        }
    }
}

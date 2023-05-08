using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using WebbShop.Data;
using WebbShop.Models;
using WebbShop.Session;

namespace WebbShop.Pages
{
    public class CartPageModel : PageModel
    {
        public List<Product> products { get; set; } = new List<Product>();
        public List<Product> CartListProducts { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<ImageFile> Images { get; set; } = new List<ImageFile>();
        public decimal Sum { get; set; }

        public readonly ApplicationDbContext _dbcontext;

        public class CarListProduct
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public ImageFile ImageFile { get; set; }
        }

        public CartPageModel(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            products = _dbcontext.product.ToList();
            Categories = _dbcontext.category.ToList();
            Images = _dbcontext.imagefiles.ToList();
        }
        public IActionResult OnGet()
        {
            var item = HttpContext.Session.Get<List<CarListProduct>>("CartList");
            if (item != null)
            {
                foreach (var items in item)
                {
                    CartListProducts.Add(_dbcontext.product.First(c => c.Id == items.Id));
                }
                foreach (var p in CartListProducts)
                {
                    Sum += p.Price;
                }
                ViewData["Amount"] = item.Count();
            }
            return Page();

        }
        public IActionResult OnGetAddToCart(int id, string returnUrl)
        {
            List<CarListProduct> CartList = HttpContext.Session.Get<List<CarListProduct>>("CartList") != null ? (List<CarListProduct>)HttpContext.Session.Get<List<CarListProduct>>("CartList") : new List<CarListProduct>();

            if (!CartList.Any(p => p.Id == id))
            {
                CartList.Add(new CarListProduct()
                {
                    Id = _dbcontext.product.First(c => c.Id == id).Id,
                });
                //CartList.Add(new CarListProduct() 
                //{
                //    Name = _dbcontext.product.First(n => n.Id == id).Name,
                //    Description = _dbcontext.product.First(n => n.Id == id).Description,
                //    ImageFile = _dbcontext.imagefiles.First(im => im.product.Id == id),
                //    Price = _dbcontext.product.First(n => n.Id == id).Price
                //});
            }
            HttpContext.Session.Set("CartList", CartList);
            returnUrl = HttpContext.Request.Path.ToString();
            return Redirect(returnUrl);
            //return RedirectToPage("Index");
        }
        public IActionResult OnGetDelete(int id)
        {
            List<CarListProduct> CartList = HttpContext.Session.Get<List<CarListProduct>>("CartList") != null ? (List<CarListProduct>)HttpContext.Session.Get<List<CarListProduct>>("CartList") : new List<CarListProduct>();

            CartList.Remove(CartList.First(c => c.Id == id));
            HttpContext.Session.Set("CartList", CartList);
            return OnGet();
        }
    }
}

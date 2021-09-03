using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public readonly ApplicationDbContext _dbcontext;

        public class CarListProduct
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImageFile { get; set; }
            public string Category { get; set; }
        }

        public CartPageModel(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            products = _dbcontext.product.ToList();
            Categories = _dbcontext.category.ToList();
            Images = _dbcontext.imagefiles.ToList();
        }
        public IActionResult OnGet(int id)
        {
            if (id != 0)
            {
                AddToCartList(id);
            }
            else
            {
                //products = HttpContext.Session.Get<List<Product>>("CarList").ToList();
                var item = HttpContext.Session.Get<List<Product>>("CartList");
                if (item != null)
                {
                    foreach (var items in item)
                    {
                        CartListProducts.Add(_dbcontext.product.First(c => c.Id == items.Id));
                    }
                    ViewData["Amount"] = item.Count();
                }
            }
            return Page();

        }
        public void AddToCartList(int id)
        {
            List<CarListProduct> CardList = HttpContext.Session.Get<List<CarListProduct>>("CartList") != null ? (List<CarListProduct>)HttpContext.Session.Get<List<CarListProduct>>("CartList") : new List<CarListProduct>();

            if (!CardList.Any(p => p.Id == id))
            {
                CardList.Add(new CarListProduct()
                {
                    Id = _dbcontext.product.First(c => c.Id == id).Id,
                });
            }
            HttpContext.Session.Set("CartList", CardList);


            //CartListProducts.Add(CartList);
            //var sessionStore = HttpContext.Session.Get<List<Product>>("CarList").ToList();
            //if (sessionStore != null)
            //{
            //    var products = HttpContext.Session.Get<List<Product>>("CarList").ToList();
            //    CartListProducts.Add(product);
            //}
            //HttpContext.Session.Set<List<Product>>("CartList", CartListProducts);

            //products = HttpContext.Session.Set("CarList", products);
            //ViewData["Cart"] = HttpContext.Session.Get("CartList");


            //return RedirectToPage("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Pages.Admin.Management
{
    [BindProperties]
    [Authorize(Roles = "Admin,ProductManager")]
    public class CreateProductModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly INotyfService _notifikation;
        public CreateProductModel(ApplicationDbContext dbContext, INotyfService notyf)
        {
            _notifikation = notyf;
            _dbContext = dbContext;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int SelectedCategoryId { get; set; }
        [Required]
        public IFormFile Bild { get; set; }

        public List<SelectListItem> Category { get; set; }

        public string FileName { get; set; }
        public void OnGet()
        {
            Category = _dbContext.category.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Name = Name,
                    Description = Description,
                    Price = Price,
                    Category = _dbContext.category.First(c => c.Id == SelectedCategoryId)
                };
                _dbContext.product.Add(product);
                _dbContext.SaveChanges();
                SaveImageToDb(product.Id);
                _notifikation.Success("Your new product is now added to database", 3);
                return RedirectToPage("/Admin/AdminPage");                
            }
            _notifikation.Warning("Please enter information!",3);
            return Page();
        }
        public void SaveImageToDb(int n)
        {
            ImageFile img = new ImageFile();
            var prodcut = _dbContext.product.First(c => c.Id == n);
            if (Bild is not null)
            {
                foreach (var file in Request.Form.Files)
                {
                    img.Filename = file.FileName;
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.DataFiles = ms.ToArray();
                    img.product = prodcut;

                    ms.Close();
                    ms.Dispose();

                    _dbContext.imagefiles.Add(img);
                }
            }
            _dbContext.SaveChanges();
        }
    }
}

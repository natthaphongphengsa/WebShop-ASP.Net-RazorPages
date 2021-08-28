using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize(Roles = "Admin")]
    public class CreateProductModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IHostingEnvironment ihostingEnvironment;

        public CreateProductModel(ApplicationDbContext dbContext, IHostingEnvironment ihostingEnvironment)
        {
            _dbContext = dbContext;
            this.ihostingEnvironment = ihostingEnvironment;
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
        private List<Category> categories { get; set; } = new List<Category>();

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
            OnGet();
            var newFileName = "";
            if (Bild is not null)
            {
                var fileName = Path.GetFileName(Bild.FileName);
                var fileExtension = Path.GetExtension(fileName);
                newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                //var imagefile = new ImageStore()
                //{
                //    Name = newFileName,
                //    Filtype = fileExtension,
                //};
                using (var target = new MemoryStream())
                {
                    Bild.CopyTo(target);
                }
            }
            else
            {
            }
            if (ModelState.IsValid)
            {
                _dbContext.product.Add(new Product() 
                {
                    Name = Name,
                    Description = Description,
                    Image = newFileName,
                    Price = Price,
                    Category = _dbContext.category.First(c => c.Id == SelectedCategoryId)
                });
                _dbContext.SaveChanges();
                return RedirectToPage("/Admin/AdminPage");
            }
            return Page();
        }
    }
}

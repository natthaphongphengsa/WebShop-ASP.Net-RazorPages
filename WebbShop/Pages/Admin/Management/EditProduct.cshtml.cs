using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class EditProductModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        public EditProductModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        [Required]
        public int SelectedCategoryId { get; set; }

        public IFormFile Bild { get; set; }
        public List<SelectListItem> Category { get; set; }
        private List<Category> categories { get; set; } = new List<Category>();

        public void OnGet(int id)
        {
            categories = _dbContext.category.ToList();
            var item = _dbContext.product.First(i => i.Id == id);
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Image = item.Image;
            SelectedCategoryId = item.Category.Id;

            Category = _dbContext.category.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }
        public IActionResult OnPost(int id)
        {
            OnGet(id);
            //string filename = Path.GetFileNameWithoutExtension(GetImageFileName);
            //string extension = Path.GetExtension(GetImageFileName);
            //ImagePath = "~/Image/" + filename;

            if (ModelState.IsValid)
            {
                var updateproduct = _dbContext.product.First(p => p.Id == id);
                updateproduct.Name = Name;
                updateproduct.Description = Description;
                updateproduct.Price = Price;
                if (Image != Bild.FileName)
                {
                    updateproduct.Image = Bild.FileName;                    
                }
                updateproduct.Category = _dbContext.category.First(c => c.Id == SelectedCategoryId);
                _dbContext.SaveChanges();
                return RedirectToPage("/Admin/AdminPage");
            }
            return Page();
        }
    }
}

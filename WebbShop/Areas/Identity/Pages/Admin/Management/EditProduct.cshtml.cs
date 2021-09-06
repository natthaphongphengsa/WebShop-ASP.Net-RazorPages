using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebbShop.Data;
using WebbShop.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebbShop.Pages.Admin.Management
{
    [BindProperties]
    [Authorize(Roles = "Admin,ProductManager")]
    public class EditProductModel : PageModel
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly INotyfService notyf;
        public EditProductModel(ApplicationDbContext dbContext, INotyfService notyfService)
        {
            notyf = notyfService;
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

        public void OnGet(int id)
        {
            var item = _dbContext.product.Include(c=> c.Category).ToList().First(p => p.Id == id);
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;            
            SelectedCategoryId = item.Category.Id;
            var img = _dbContext.imagefiles.First(c => c.product == item);
            if (img.DataFiles == null) 
            {
                Image = img.Filename;
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.DataFiles);
                string ImageUrl = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                Image = ImageUrl;

            }
            Category = _dbContext.category.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }
        public IActionResult OnPost(int id)
        {            
            if (ModelState.IsValid)
            {
                var updateproduct = _dbContext.product.First(p => p.Id == id);
                updateproduct.Name = Name;
                updateproduct.Description = Description;
                updateproduct.Price = Price;              
                updateproduct.Category = _dbContext.category.First(c => c.Id == SelectedCategoryId);
                _dbContext.product.Update(updateproduct);
                _dbContext.SaveChanges();

                if (Bild != null)
                {
                    var imageid = _dbContext.imagefiles.First(i => i.product.Id == id);
                    UpdateImageInDb(imageid.Id);
                }

                notyf.Success("Your product is now updated", 3);
                return RedirectToPage("/Admin/AdminPage");
                //return RedirectToPage("/Admin/Management/Confirm", new { text = "Your product is now updateded", id = 1 });
            }
            OnGet(id);
            notyf.Error("Sorry something went wrong. Try again!", 3);
            return Page();
        }
        public void UpdateImageInDb(int id)
        {
            if (Bild is not null)
            {
                foreach (var file in Request.Form.Files)
                {                    
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);

                    ms.Close();
                    ms.Dispose();

                    var upimg = _dbContext.imagefiles.First(i => i.Id == id);
                    upimg.Filename = file.FileName;
                    upimg.DataFiles = ms.ToArray();
                    _dbContext.imagefiles.Update(upimg);                    
                    _dbContext.SaveChanges();
                }
            }
            
        }
    }
}

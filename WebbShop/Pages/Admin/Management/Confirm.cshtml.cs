using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebbShop.Pages.Admin.Management
{
    [Authorize(Roles = "Admin")]
    public class ConfirmModel : PageModel
    {
        public string Text { get; set; }
        public int Id { get; set; }
        public void OnGet(string text, int id)
        {
            Text = text;
            Id = id;
        }        
    }
}

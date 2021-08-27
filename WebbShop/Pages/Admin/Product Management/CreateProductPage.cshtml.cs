using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebbShop.Pages.Admin.Product_Management
{
    [Authorize(Roles = "Admin")]
    public class CreateProductPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

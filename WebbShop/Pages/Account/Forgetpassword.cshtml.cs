using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebbShop.Pages.Account
{
    public class ForgetpasswordModel : PageModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            string code = Guid.NewGuid().ToString();
            MailMessage mail = new MailMessage("WebbShop@outlook.com", EmailAddress);
            mail.IsBodyHtml = true;
            mail.Subject = "WebbShop";
            mail.Body = $"Your reset code is:{code}";

            NetworkCredential netCred = new NetworkCredential("WebbShop@outlook.com", "9876543210abc");
            SmtpClient smtpobj = new SmtpClient("outlook.office365.com", 587);
            smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            await smtpobj.SendMailAsync(mail);

            return RedirectToPage("/Account/Resetpassword");
        }
    }
}

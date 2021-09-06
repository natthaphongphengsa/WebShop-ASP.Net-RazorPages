using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebbShop.Data;
using WebbShop.Models;

namespace WebbShop.Data
{
    public class DataInitializer
    {
        public static void SeedData(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SeedAdminAsync(dbContext);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                              //SeedCategory(dbContext);
                              //SeedProducts(dbContext);
        }
        public static void SeedProducts(ApplicationDbContext dbContext)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var json = webClient.DownloadString(@"https://fakestoreapi.com/products/");
            var products = JsonConvert.DeserializeObject<List<ProductsAPI>>(json);

            foreach (var item in products)
            {
                if (!dbContext.product.Any(c => c.Name == item.title))
                {
                    var product  = new Product()
                    {
                        Name = item.title,
                        Description = item.description,
                        Price = item.price,
                        Category = dbContext.category.FirstOrDefault(c => c.Name == item.category)
                    };
                    dbContext.product.Add(product);
                    dbContext.SaveChanges();
                    dbContext.imagefiles.Add(new ImageFile() 
                    {
                       Filename = item.image,
                       product = product,
                    });
                    dbContext.SaveChanges();
                }
            }
            //dbContext.SaveChanges();
        }
        public static void SeedCategory(ApplicationDbContext dbContext)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var json = webClient.DownloadString(@"https://fakestoreapi.com/products/");
            var Products = JsonConvert.DeserializeObject<List<ProductsAPI>>(json);

            foreach (var name in Products)
            {
                if (!dbContext.category.Any(c => c.Name == name.category))
                {
                    dbContext.category.Add(new Category() { Name = name.category });
                    dbContext.SaveChanges();
                }
            }
        }
        public static async Task SeedAdminAsync(ApplicationDbContext dbContext)
        {
            var Roles = new RoleStore<IdentityRole>(dbContext);
            if (!dbContext.Roles.Any(r => r.Name == "Admin"))
            {
                await Roles.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }
            if (!dbContext.Roles.Any(r => r.Name == "Customer"))
            {
                await Roles.CreateAsync(new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" });
            }
            if (!dbContext.Roles.Any(r => r.Name == "ProductManager"))
            {
                await Roles.CreateAsync(new IdentityRole { Name = "ProductManager", NormalizedName = "PRODUCTMANAGER" });
            }

            var Admin = new IdentityUser()
            {
                UserName = "Admin@hotmail.com",
                NormalizedUserName = "ADMIN@HOTMAIL.COM",
                NormalizedEmail = "ADMIN@HOTMAIL.COM",
                Email = "Admin@hotmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            if (!dbContext.Users.Any(u => u.UserName == Admin.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(Admin, "Admin@");
                Admin.PasswordHash = hashed;

                dbContext.Users.Add(Admin);
                var UserRole = new IdentityUserRole<string>() 
                {
                    RoleId = dbContext.Roles.First( c=> c.Name == "Admin").Id,
                    UserId = Admin.Id
                };
                dbContext.UserRoles.Add(UserRole);
            }
            var ProductManager = new IdentityUser()
            {
                UserName = "ProductManager@hotmail.com",
                NormalizedUserName = "PRODUCTMANAGER@HOTMAIL.COM",
                NormalizedEmail = "PRODUCTMANAGER@HOTMAIL.COM",
                Email = "ProductManager@hotmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!dbContext.Users.Any(u => u.UserName == ProductManager.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(ProductManager, "ProductManager@");
                ProductManager.PasswordHash = hashed;

                dbContext.Users.Add(ProductManager);
                var UserRole = new IdentityUserRole<string>()
                {
                    RoleId = dbContext.Roles.First(c => c.Name == "ProductManager").Id,
                    UserId = ProductManager.Id
                };
                dbContext.UserRoles.Add(UserRole);
            }
            dbContext.SaveChanges();
        }
    }
}

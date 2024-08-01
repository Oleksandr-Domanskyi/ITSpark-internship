using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity.Product;
using ApplicationCore.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInfrastructure.Data.Seed
{
    public class ApplicationSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationSeeder(ApplicationDbContext dbContext,
                                UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.ItemProfile.Any())
                {
                    var SeedModel = new Product
                    {
                        Name = "MrBeast",
                        Description = "The statuette of MrBeast",
                        Category = Category.Figure.ToString(),
                        Price = 10.49,
                    };
                    await _dbContext.ItemProfile.AddAsync(SeedModel);
                    await _dbContext.SaveChangesAsync();
                }


                string[] roles = { UserRole.Admin.ToString(), UserRole.Customer.ToString() };
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                if (_userManager.Users.All(u => u.UserName != "Admin@gmail.com"))
                {
                    var user = new IdentityUser { UserName = "Admin@gmail.com", Email = "Admin@gmail.com" };
                    await _userManager.CreateAsync(user, "123456Sd#");
                    await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                }

            }
        }
    }
}

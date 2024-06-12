using ApplicationCore.Domain.Entity.ItemProfile;
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

        public ApplicationSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.ItemProfile.Any())
                {
                    var SeedModel = new ItemProfile
                    {
                        Name = "MrBeast",
                        Description = "The statuette of MrBeast",
                        Category = ApplicationCore.Domain.Enum.Category.Figure.ToString(),
                        Price = 10.49,
                    };
                    await _dbContext.ItemProfile.AddAsync(SeedModel);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

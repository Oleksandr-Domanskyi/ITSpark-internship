using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using ApplicationInfrastructure.Contracts;

namespace ApplicationInfrastructure.Services.ImageService.GetOldImagePathService
{
    public class OldImagePathService<TDomain> : IOldImagePathService<TDomain>
    where TDomain : Entity<Guid>
    {
        public List<Image> GetOldImagePath(TDomain entity)
        {
            var OldListPath = new List<Image>();
            var properties = typeof(TDomain).GetProperties().Where(p => p.PropertyType == typeof(List<Image>));
            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    if (property != null)
                    {
                         OldListPath = (List<Image>)property.GetValue(entity)!;
                    }
                   
                }
            }
            return OldListPath;

        }
    }
}
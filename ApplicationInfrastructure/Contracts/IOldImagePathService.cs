using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;

namespace ApplicationInfrastructure.Contracts
{
    public interface IOldImagePathService<TDomain>
    where TDomain : Entity<Guid>
    {
        public List<Image> GetOldImagePath (TDomain entity);
    }
}
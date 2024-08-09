using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;

namespace Applications.Contracts
{
    public interface IUserAccessManagerService<TDomain, TDto>
    where TDomain : Entity<Guid>
    where TDto : class
    {
        public Task<bool> CheckUserAccessAsync(Guid id);
        public Task<Filters> GenerateFiltersBasedOnUser(FiltersOption option);
    }
}
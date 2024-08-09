using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Dto.Response;

namespace Applications.Contracts
{
    public interface IFilterManagerService
    {
        public Filters GenerateFiltersBasedOnUserRole(FiltersOption filtersOption, UserResponse user);
    }
}
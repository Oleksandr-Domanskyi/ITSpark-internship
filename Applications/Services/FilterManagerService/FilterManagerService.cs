using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Enum;
using ApplicationCore.Dto.Response;
using Applications.Contracts;
using AutoMapper;

namespace Applications.Services.FilterManagerService
{
    public class FilterManagerService : IFilterManagerService
    {
        private readonly IMapper _mapper;
        public FilterManagerService(IMapper mapper)
        {
            _mapper = mapper;

        }
        public Filters GenerateFiltersBasedOnUserRole(FiltersOption filtersOption, UserResponse user)
        {
            var filters = _mapper.Map<Filters>(filtersOption);
            filters.AddUserInformation(user);
            return filters;
        }
    }
}
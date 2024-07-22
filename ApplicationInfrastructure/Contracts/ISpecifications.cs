using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using Ardalis.Specification;

namespace ApplicationInfrastructure.Contracts
{
    public interface ISpecifications<T> where T : Entity<Guid>
    {
        IQueryable<T> ApplyInclude(IQueryable<T> query);
        IQueryable<T> ApplyFilter(IQueryable<T> query, Filters filters);

    }
}
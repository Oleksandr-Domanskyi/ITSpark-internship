using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using Ardalis.Specification;

namespace ApplicationInfrastructure.Contracts
{
    public interface ISpecifications<T> where T : Entity<Guid>
    {
        IQueryable<T> ApplyInclude(IQueryable<T> query);
    }
}
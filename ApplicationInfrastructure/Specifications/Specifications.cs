using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using ApplicationInfrastructure.Contracts;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Enum;

namespace ApplicationInfrastructure.Specifications
{
    public class AutoSpecification<T> : ISpecifications<T>
        where T : Entity<Guid>
    {
        public IQueryable<T> ApplyInclude(IQueryable<T> query)
        {
            var properties = typeof(T).GetProperties();

            var propertiesToInclude = properties
                .Where(property => IsIncludedNavigationProperty(property))
                .Select(property => property.Name)
                .ToList();

            query = propertiesToInclude.Aggregate(query, (currentQuery, propertyName) => currentQuery.Include(propertyName));

            return query;
        }
        public IQueryable<T> ApplyFilter(IQueryable<T> query, Filters filters)
        {
            if (filters.Roles == UserRole.Customer.ToString())
            {
                query = query.Where(item => item.CreatedBy == filters.CreatedBy);
            }
            return query;         
        }


        private bool IsIncludedNavigationProperty(PropertyInfo property)
        {
            return property.PropertyType == typeof(List<Image>);
        }
    }
}

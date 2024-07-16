using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using ApplicationInfrastructure.Contracts;

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

        private bool IsIncludedNavigationProperty(PropertyInfo property)
        {
            return property.PropertyType == typeof(List<Image>);
        }
    }
}

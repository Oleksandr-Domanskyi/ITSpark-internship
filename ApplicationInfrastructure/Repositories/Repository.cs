using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.Image;
using ApplicationCore.Domain.Enum;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Data;
using Applications.Contracts;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationInfrastructure.Repositories
{
    public class Repository<T> : RepositoryBase<T>, IRepositoryContract<T> where T : Entity<Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<T>> ListAsync(Filters filters, ISpecifications<T> specifications, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            int skipAmount = (filters.CurrentPage - 1) * filters.perPage;
            if (skipAmount < 0 || filters.CurrentPage == 0)
            {
                return new List<T>();
            }

            if (specifications != null)
            {
                query = specifications.ApplyInclude(query);
                query = specifications.ApplyFilter(query,filters);
            }

            if (!string.IsNullOrEmpty(filters.SearchBy))
            {
                query = query
                    .Where(item =>
                     EF.Property<string>(item, typeof(T)
                        .GetProperties()
                        .ElementAtOrDefault(0)!.Name)
                        .Contains(filters.SearchBy));
            }

            PropertyInfo[] numericProperties = typeof(T)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(double))
                .ToArray();

            if (numericProperties.Any()
                && filters.ToStartSearch != null
                && filters.ToEndSearch != null)
            {
                var filteredItems = await query.ToListAsync(cancellationToken);

                filteredItems = filteredItems
                    .Where(item =>
                    {
                        foreach (var property in numericProperties)
                        {
                            var propertyValue = property.GetValue(item);
                            if (propertyValue == null) continue;

                            double? value = (double?)propertyValue;
                            bool startCondition = filters.ToStartSearch == null || value >= filters.ToStartSearch;
                            bool endCondition = filters.ToEndSearch == null || value <= filters.ToEndSearch;

                            if (!(startCondition && endCondition))
                            {
                                return false;
                            }
                        }
                        return true;
                    })
                    .Skip(skipAmount)
                    .Take(filters.perPage)
                    .ToList();

                if (filters.SortDirection == null || filters.SortDirection == SortDirection.Ascending.ToString())
                {
                    return filteredItems
                        .OrderBy(x =>
                            typeof(T).GetProperties().ElementAtOrDefault(0)!.Name)
                        .ToList();
                }
                else
                {
                    return filteredItems
                        .OrderByDescending(x =>
                             typeof(T).GetProperties().ElementAtOrDefault(0)!.Name)
                        .ToList();
                }
            }

            if (filters.SortDirection == null || filters.SortDirection == SortDirection.Ascending.ToString())
            {
                return await query
                    .Skip(skipAmount)
                    .Take(filters.perPage)
                    .OrderBy(x =>
                        EF.Property<string>(x, typeof(T).GetProperties().ElementAtOrDefault(0)!.Name))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                return await query
                    .Skip(skipAmount)
                    .Take(filters.perPage)
                    .OrderByDescending(x =>
                        EF.Property<string>(x, typeof(T).GetProperties().ElementAtOrDefault(0)!.Name))
                    .ToListAsync(cancellationToken);
            }
        }
        public async Task<T?> GetByIdAsync<TId>(TId id, ISpecifications<T> specifications, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (specifications != null)
            {
                query = specifications.ApplyInclude(query);
            }
            return await query.FirstOrDefaultAsync(p => p.Id.ToString() == id!.ToString());
        }

        public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = new())
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }
        public override async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {

            var existingEntity = await _dbContext.Set<T>().FindAsync(entity.Id);
            if (existingEntity == null)
            {
                throw new InvalidOperationException($"Entity with id {entity.Id} not found.");
            }

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<T?> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<List<T?>> DeleteRangeAsync(List<T> entity, CancellationToken cancellationToken)
        {
            if (entity.Any())
            {
                _dbContext.Set<T>().RemoveRange(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return entity!;
        }
        public async Task<List<T>> AddRange(List<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities.Any())
            {
                await _dbContext.Set<T>().AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
            }
            return entities;
        }
    }
}

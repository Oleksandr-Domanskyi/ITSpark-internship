using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using Applications.Contracts;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationInfrastructure.Repositories
{
    public class Repository<T> : RepositoryBase<T>, IRepositoryContract<T> where T : Entity<Guid>
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
            =>await _dbContext.Set<T>().ToListAsync(cancellationToken);

        public override async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
               => await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = new())
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }
        public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
             _dbContext.Set<T>().Update(entity);
        }
       

        public async Task<T?> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}

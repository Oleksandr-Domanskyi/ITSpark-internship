using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Data;
using Applications.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInfrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly Hashtable _repositories = new();
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;

        }

        public IRepositoryContract<T> Repository<T>() where T : Entity<Guid>
        {
            var type = typeof(T).Name;
            if (_repositories.ContainsKey(type)) return (IRepositoryContract<T>)_repositories[type]!;

            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
            _repositories.Add(type, repositoryInstance);

            return (IRepositoryContract<T>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync()
        {
            var res = await _dbContext.SaveChangesAsync();
            return res;
        }
    }
}

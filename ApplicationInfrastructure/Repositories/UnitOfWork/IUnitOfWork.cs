using ApplicationCore.Domain.Entity;
using Applications.Contracts;

namespace ApplicationInfrastructure.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        IRepositoryContract<T> Repository<T>() where T : Entity<Guid>;
    }
}
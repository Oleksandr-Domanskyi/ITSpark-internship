using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationInfrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Contracts
{
    public interface IRepositoryContract<T> where T : Entity<Guid>
    {
        Task<List<T>> ListAsync(FiltersOption filters, ISpecifications<T> specification, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = new());
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

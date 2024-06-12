using ApplicationCore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Contracts
{
    public interface IRepositoryContract<T> where T : Entity<Guid>
    {
        Task<List<T>> ListAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = new());
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

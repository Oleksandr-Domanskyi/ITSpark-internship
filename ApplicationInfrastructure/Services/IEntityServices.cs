using ApplicationCore.Domain.Entity;
using FluentResults;

namespace ApplicationInfrastructure.Services
{
    public interface IEntityService<EntityType> where EntityType : Entity<Guid>
    {
        Task<Result<List<EntityType>>> GetListAsync();
        Task<Result<EntityType>> GetByIdAsync(Guid id);
        Task<Result<EntityType>> AddEntityAsync(EntityType entity);
        Task<Result<EntityType>> UpdateAsync(EntityType entity);
        Task<Result<EntityType>> DeleteAsync(Guid id);
    }
}
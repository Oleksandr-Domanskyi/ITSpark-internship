using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using FluentResults;

namespace ApplicationInfrastructure.Services
{
    public interface IEntityService<EntityType,EntityDto>
     where EntityType : Entity<Guid>
     where EntityDto : class
    {
        Task<Result<Filters<EntityDto>>> GetListAsync(FiltersOption filters);
        Task<Result<EntityDto>> GetByIdAsync(Guid id);
        Task<Result<EntityType>> AddEntityAsync(EntityDto entity);
        Task<Result<EntityType>> UpdateAsync(EntityDto entity, Guid id);
        Task<Result<EntityType>> DeleteAsync(Guid id);
    }
}
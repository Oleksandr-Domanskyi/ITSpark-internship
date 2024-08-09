using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using FluentResults;

namespace ApplicationInfrastructure.Services
{
    public interface IEntityService<EntityType, EntityDto>
     where EntityType : Entity<Guid>
     where EntityDto : class
    {
        public Task<Result<IEnumerable<EntityDto>>> GetListAsync(Filters filters);
        public Task<Result<EntityDto>> GetByIdAsync(Guid id);
        public Task<Result<EntityType>> AddEntityAsync(EntityDto entity);
        public Task<Result<EntityType>> UpdateAsync(EntityDto entity, Guid id);
        public Task<Result<EntityType>> DeleteAsync(Guid id);
    }
}
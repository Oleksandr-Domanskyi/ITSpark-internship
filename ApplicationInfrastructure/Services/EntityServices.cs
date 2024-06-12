using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Data;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInfrastructure.Services
{
    public class EntityServices<EntityType> : IEntityService<EntityType> where EntityType : Entity<Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;

        public EntityServices(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<Result<EntityType>> AddEntityAsync(EntityType entity)
        {
            await _unitOfWork.Repository<EntityType>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<EntityType>> DeleteAsync(Guid id)
        {
            return await Result.Try(async Task<EntityType>()=>
                await _unitOfWork.Repository<EntityType>().DeleteAsync(id),
                ex=> new Error(ex.Message));
        }

        public async Task<Result<EntityType>> GetByIdAsync(Guid id)
        {
            return await Result.Try(async Task<EntityType> () =>
                await _unitOfWork.Repository<EntityType>().GetByIdAsync(id),
                ex => new Error(ex.Message));
        }

        public async Task<Result<List<EntityType>>> GetListAsync()
        {
            return await Result.Try(async Task<List<EntityType>> () =>
                await _unitOfWork.Repository<EntityType>().ListAsync(),
                ex => new Error(ex.Message));
        }

        public async Task<Result<EntityType>> UpdateAsync(EntityType entity)
        {
            await _unitOfWork.Repository<EntityType>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}

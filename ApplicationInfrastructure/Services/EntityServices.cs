using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.ItemProfile;
using ApplicationInfrastructure.Data;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using ApplicationInfrastructure.Services.ImageService;
using ApplicationInfrastructure.Specifications;
using Applications.Services.FilterService;
using AutoMapper;
using AutoMapper.Internal;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInfrastructure.Services
{
    public class EntityServices<EntityType, EntityDto> : IEntityService<EntityType, EntityDto>
     where EntityType : Entity<Guid>
     where EntityDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageAzureService<EntityType, EntityDto> _imageAzureService;

        public EntityServices(IUnitOfWork unitOfWork, IMapper mapper,
         IImageAzureService<EntityType, EntityDto> imageAzureService)
        {
            _imageAzureService = imageAzureService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EntityType>> AddEntityAsync(EntityDto entity)
        {
            IEnumerable<IFormFile> images;
            var Domain = _mapper.Map<EntityType>(entity);

            if (_imageAzureService.HaveImages(entity, out images))
            {
                var Path = await _imageAzureService.UploadImagesToAzure(images.ToList());
                Domain = _imageAzureService.SetImagePath(Domain, Path);
            }
            await _unitOfWork.Repository<EntityType>().AddAsync(Domain);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<EntityType>> DeleteAsync(Guid id)
        {
            return await Result.Try(async Task<EntityType> () =>
                await _unitOfWork.Repository<EntityType>().DeleteAsync(id),
                ex => new Error(ex.Message));
        }

        public async Task<Result<EntityDto>> GetByIdAsync(Guid id)
        {
            var model = await Result.Try(async Task<EntityType> () =>
                 await _unitOfWork.Repository<EntityType>().GetByIdAsync(id),
                 ex => new Error(ex.Message));
            return _mapper.Map<EntityDto>(model);
        }

        public async Task<Result<Filters<EntityDto>>> GetListAsync(FiltersOption filters)
        {
            var specification = new AutoSpecification<EntityType>();
            var entity = await _unitOfWork.Repository<EntityType>().ListAsync(filters, specification);


            var filteredEntities = new Filters<EntityDto>();
            filteredEntities.AddFilterOption(filters, _mapper.Map<List<EntityDto>>(entity));

            return Result.Ok(filteredEntities);
        }


        public async Task<Result<EntityType>> UpdateAsync(EntityDto entity)
        {
            await _unitOfWork.Repository<EntityType>().UpdateAsync(_mapper.Map<EntityType>(entity));
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}

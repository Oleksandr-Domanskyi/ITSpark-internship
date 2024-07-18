using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.Image;
using ApplicationCore.Domain.Entity.ItemProfile;
using ApplicationInfrastructure.Contracts;
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
        private readonly AutoSpecification<EntityType> specification = new AutoSpecification<EntityType>();


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageAzureService<EntityType, EntityDto> _imageAzureService;
        private readonly IOldImagePathService<EntityType> _oldImagePathService;

        public EntityServices(IUnitOfWork unitOfWork, IMapper mapper,
         IImageAzureService<EntityType, EntityDto> imageAzureService,
         IOldImagePathService<EntityType> oldImagePathService)
        {
            _imageAzureService = imageAzureService;
            _oldImagePathService = oldImagePathService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EntityType>> AddEntityAsync(EntityDto entity)
        {
            List<IFormFile> images;
            var Domain = _mapper.Map<EntityType>(entity);

            if (_imageAzureService.HaveImages(entity, out images))
            {
                var Path = await _imageAzureService.UploadImagesToAzure(images);
                Domain = _imageAzureService.SetImagePath(Domain, Path);
            }
            var responce = await _unitOfWork.Repository<EntityType>().AddAsync(Domain);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok(responce);
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
                 await _unitOfWork.Repository<EntityType>().GetByIdAsync(id, specification),
                 ex => new Error(ex.Message));
            return Result.Ok(_mapper.Map<EntityDto>(model.Value));
        }

        public async Task<Result<IEnumerable<EntityDto>>> GetListAsync(FiltersOption filters)
        {
            var entity = await _unitOfWork.Repository<EntityType>().ListAsync(filters, specification);
            return Result.Ok(_mapper.Map<IEnumerable<EntityDto>>(entity));
        }


        public async Task<Result<EntityType>> UpdateAsync(EntityDto entity, Guid id)
        {
            List<IFormFile> images;
            var specification = new AutoSpecification<EntityType>();
            var Domain = await _unitOfWork.Repository<EntityType>().GetByIdAsync(id, specification);

            if (_imageAzureService.HaveImages(entity, out images))
            {
                //Delete Old Image
                await _imageAzureService.DeleteRangeOldImageFromAzure(Domain!);
                var oldImagePath = _oldImagePathService.GetOldImagePath(Domain!);
                await _unitOfWork.Repository<Image>().DeleteRangeAsync(oldImagePath);

                //Save new Image
                var Path = await _imageAzureService.UploadImagesToAzure(images);
                Path = _imageAzureService.SetImageItemProfileId(Path, id);
                await _unitOfWork.Repository<Image>().AddRange(Path);
                Domain = _imageAzureService.SetImagePath(_mapper.Map<EntityType>(entity), Path);
            }
            else
            {
                Domain = _mapper.Map<EntityType>(entity);
            }
            Domain!.Id = id;
            var response = await _unitOfWork.Repository<EntityType>().UpdateAsync(Domain!);

            return Result.Ok(response);
        }
    }
}

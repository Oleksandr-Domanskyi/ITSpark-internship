using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.Image;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using ApplicationInfrastructure.Services.ImageService;
using ApplicationInfrastructure.Specifications;
using Applications.Contracts;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http;


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
        private readonly IDeleteImageFromAzureEvent<EntityType, EntityDto> _deleteImageFromAzureEvent;

        public EntityServices(IUnitOfWork unitOfWork, IMapper mapper,
         IImageAzureService<EntityType, EntityDto> imageAzureService,
         IOldImagePathService<EntityType> oldImagePathService,
         IDeleteImageFromAzureEvent<EntityType,EntityDto> deleteImageFromAzureEvent) 
        {
            _imageAzureService = imageAzureService;
            _oldImagePathService = oldImagePathService;
            _deleteImageFromAzureEvent = deleteImageFromAzureEvent;
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
            var entity = await _unitOfWork.Repository<EntityType>().GetByIdAsync(id, specification);

            _deleteImageFromAzureEvent.ImageDeleteEvent(entity!);

            await _unitOfWork.Repository<EntityType>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok(entity!);
        }

        public async Task<Result<EntityDto>> GetByIdAsync(Guid id)
        {
            var model = await Result.Try(async Task<EntityType> () =>
                 await _unitOfWork.Repository<EntityType>().GetByIdAsync(id, specification),
                 ex => new Error(ex.Message));
            return Result.Ok(_mapper.Map<EntityDto>(model.Value));
        }

        public async Task<Result<IEnumerable<EntityDto>>> GetListAsync(Filters filters)
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
                
                //Delete Old Images
                _deleteImageFromAzureEvent.ImageDeleteEvent(Domain!);
                var oldImagePath = _oldImagePathService.GetOldImagePath(Domain!);
                await _unitOfWork.Repository<Image>().DeleteRangeAsync(oldImagePath);

                //Save new Images
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

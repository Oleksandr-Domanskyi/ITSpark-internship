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
        private readonly IImageManagerService<EntityType, EntityDto> _imageManagerService;
        private readonly IOldImagePathService<EntityType> _oldImagePathService;
        private readonly IDeleteImageFromAzureEvent<EntityType, EntityDto> _deleteImageFromAzureEvent;

        public EntityServices(IUnitOfWork unitOfWork, IMapper mapper,
                            IImageAzureService<EntityType, EntityDto> imageAzureService,
                            IImageManagerService<EntityType, EntityDto> imageManagerService,
                            IOldImagePathService<EntityType> oldImagePathService,
                            IDeleteImageFromAzureEvent<EntityType, EntityDto> deleteImageFromAzureEvent)
        {
            _imageAzureService = imageAzureService;
            _imageManagerService = imageManagerService;
            _oldImagePathService = oldImagePathService;
            _deleteImageFromAzureEvent = deleteImageFromAzureEvent;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<EntityType>> AddEntityAsync(EntityDto entityDto)
        {
            var domainEntity = _mapper.Map<EntityType>(entityDto);
            domainEntity = await _imageManagerService.HandleImageUploadAsync(domainEntity, entityDto);

            var response = await _unitOfWork.Repository<EntityType>().AddAsync(domainEntity);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok(response);
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
                 (await _unitOfWork.Repository<EntityType>().GetByIdAsync(id, specification))!,
                 ex => new Error(ex.Message));
            return Result.Ok(_mapper.Map<EntityDto>(model.Value));
        }

        public async Task<Result<IEnumerable<EntityDto>>> GetListAsync(Filters filters)
        {
            var entity = await _unitOfWork.Repository<EntityType>().ListAsync(filters, specification);
            return Result.Ok(_mapper.Map<IEnumerable<EntityDto>>(entity));
        }

        public async Task<Result<EntityType>> UpdateAsync(EntityDto entityDto, Guid id)
        {
            var Domain = await _unitOfWork.Repository<EntityType>().GetByIdAsync(id, specification);
            await _imageManagerService.HandleImageUpdateAsync(Domain!, entityDto, id);

            Domain = _mapper.Map<EntityType>(entityDto);
            Domain.Id = id;

            var response = await _unitOfWork.Repository<EntityType>().UpdateAsync(Domain!);

            return Result.Ok(response);
        }
    }
}

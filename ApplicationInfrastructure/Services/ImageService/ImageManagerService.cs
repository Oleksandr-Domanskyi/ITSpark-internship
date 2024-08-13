using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using Applications.Contracts;
using Applications.Events.DeleteImageFromAzure;
using Microsoft.AspNetCore.Http;

namespace ApplicationInfrastructure.Services.ImageService
{
    public class ImageManagerService<Entity, EntityDto> : IImageManagerService<Entity, EntityDto>
    where Entity : Entity<Guid>
    where EntityDto : class
    {
        private readonly IImageAzureService<Entity, EntityDto> _imageAzureService;
        private readonly IDeleteImageFromAzureEvent<Entity, EntityDto> _deleteImageFromAzureEvent;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOldImagePathService<Entity> _oldImagePathService;

        public ImageManagerService(IImageAzureService<Entity, EntityDto> imageAzureService,
                                    IDeleteImageFromAzureEvent<Entity, EntityDto> deleteImageFromAzureEvent,
                                    IUnitOfWork unitOfWork, IOldImagePathService<Entity> oldImagePathService)
        {
            _oldImagePathService = oldImagePathService;
            _deleteImageFromAzureEvent = deleteImageFromAzureEvent;
            _unitOfWork = unitOfWork;
            _imageAzureService = imageAzureService;
        }
        public bool TryGetImages(EntityDto entity, out List<IFormFile> images)
        {
            var properties = entity.GetType().GetProperties();
            var imageProps = properties.Where(p => p.PropertyType == typeof(List<IFormFile>));

            if (imageProps.Any())
            {
                var firstImageProp = imageProps.First();
                var propertyValue = firstImageProp.GetValue(entity);

                images = (List<IFormFile>)propertyValue!;
                return true;
            }
            images = default!;
            return false;
        }

        public Entity SetImagesPathToEntity(Entity entity, List<Image> Path)
        {
            if (Path == null)
            {
                return entity;
            }
            var properties = typeof(Entity).GetProperties().Where(p => p.PropertyType == typeof(List<Image>));
            foreach (var property in properties)
            {
                property.SetValue(entity, Path);
            }
            return entity;
        }
        public List<Image> SetImageItemProfileId(List<Image> images, Guid itemProfileId)
        {
            foreach (var image in images)
            {
                image.ProductId = itemProfileId;
            }
            return images;
        }
        public async Task<Entity> HandleImageUploadAsync(Entity entity, EntityDto entityDto)
        {
            if (TryGetImages(entityDto, out List<IFormFile> images))
            {
                var imagePaths = await _imageAzureService.UploadImagesToAzure(images);
                return SetImagesPathToEntity(entity, imagePaths);
            }
            return entity;
        }
        public async Task<Entity> HandleImageUpdateAsync(Entity entity, EntityDto dto, Guid id)
        {
            if (TryGetImages(dto, out List<IFormFile> images))
            {
                await DeleteOldImagesAsync(entity);
                return await UploadNewImagesAsync(entity, images, id);
            }
            return entity;
        }
        private async Task DeleteOldImagesAsync(Entity entity)
        {
            _deleteImageFromAzureEvent.ImageDeleteEvent(entity);
            var oldImagePath = _oldImagePathService.GetOldImagePath(entity);
            await _unitOfWork.Repository<Image>().DeleteRangeAsync(oldImagePath);
        }
        private async Task<Entity> UploadNewImagesAsync(Entity entity, List<IFormFile> images, Guid id)
        {
            var newImagePaths = await _imageAzureService.UploadImagesToAzure(images);
            newImagePaths = SetImageItemProfileId(newImagePaths, id);

            await _unitOfWork.Repository<Image>().AddRange(newImagePaths);
            return SetImagesPathToEntity(entity, newImagePaths);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services.ImageService;
using Applications.Contracts;
using Ardalis.Specification;

namespace Applications.Events.DeleteImageFromAzure
{
    public class DeleteImageFromAzureEvent<Entity, EntityDto> : IDeleteImageFromAzureEvent<Entity, EntityDto>
    where Entity : Entity<Guid>
    where EntityDto : class
    {
        private readonly IImageAzureService<Entity, EntityDto> _azureImageService;
        public event Func<Entity, Task> OnDeleteImage;

        public DeleteImageFromAzureEvent(IImageAzureService<Entity, EntityDto> azureImageService)
        {
            _azureImageService = azureImageService;
            OnDeleteImage += _azureImageService.DeleteRangeOldImageFromAzure;
        }

        event Func<Entity, Task> IDeleteImageFromAzureEvent<Entity, EntityDto>.OnDeleteImage
        {
            add { OnDeleteImage += value; }
            remove { OnDeleteImage -= value; }
        }

        public void ImageDeleteEvent(Entity entity)
        {
            OnDeleteImage?.Invoke(entity);
        }
    }

}
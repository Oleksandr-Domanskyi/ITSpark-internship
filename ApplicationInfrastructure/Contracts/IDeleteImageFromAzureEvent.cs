using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;

namespace Applications.Contracts
{
    public interface IDeleteImageFromAzureEvent<Entity, EntityDto>
    where Entity : Entity<Guid>
    where EntityDto : class
    {
        public event Func<Entity, Task> OnDeleteImage;
        public void ImageDeleteEvent(Entity entity);
    }
}
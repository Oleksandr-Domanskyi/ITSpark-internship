using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;

namespace Applications.Contracts
{
    public interface IDeleteImageFromAzureEvent<Entity,EntityDto>
    where Entity : Entity<Guid>
    where EntityDto: class
    {
        event Func<Entity, Task> OnDeleteImage;
        void ImageDeleteEvent(Entity entity);
    }
}
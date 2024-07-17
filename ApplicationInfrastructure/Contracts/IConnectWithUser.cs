using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;

namespace ApplicationInfrastructure.Services.ConnectWithUser
{
    public interface IConnectWithUser<TDomain>
    where TDomain : Entity<Guid>
    {
        
    }
}
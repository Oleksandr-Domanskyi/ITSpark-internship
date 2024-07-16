using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;

namespace ApplicationInfrastructure.Services.ConnectWithUser
{
    public class ConnectWithUser<TDomain> : IConnectWithUser<TDomain>
    where TDomain : Entity<Guid>
    {
        private readonly IUserContext _userContext;
        public ConnectWithUser(IUserContext userContext)
        {
            _userContext = userContext;
        }
        public TDomain ConnectUserWithProduct(TDomain domain)
        {
            PropertyInfo property = typeof(TDomain).GetProperty("CreatedBy")!;
            var id = _userContext.GetCurrentUser()?.Id;
            if (id == null)
            {
                throw new InvalidOperationException("User not logged");
            }
            var createdByValue = property.GetValue(domain)?.ToString();
            if (string.IsNullOrEmpty(createdByValue))
            {
                property.SetValue(domain, id);
            }
            return domain;
        }
    }
}
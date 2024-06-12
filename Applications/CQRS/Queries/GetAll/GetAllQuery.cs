using ApplicationCore.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Queries.GetAll
{
    public class GetAllQuery<TDomain, TDto> : IRequest<IEnumerable<TDto>>
        where TDomain : Entity<Guid>
        where TDto : class
    {

    }
}

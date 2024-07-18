using ApplicationCore.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Queries.GetById
{
    public class GetByIdQuery<TDomain,TDto>:IRequest<TDto>
        where TDomain : Entity<Guid>
        where TDto : class
    {
        public Guid _id { get; set; }
        public GetByIdQuery(Guid Id)
        {
            _id = Id;
        }
    }
}

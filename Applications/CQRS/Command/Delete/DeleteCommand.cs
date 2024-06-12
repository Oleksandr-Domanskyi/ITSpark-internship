using ApplicationCore.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Delete
{
    public class DeleteCommand<TDomain>: IRequest where TDomain : Entity<Guid>
    {
        public readonly Guid _id;
        public DeleteCommand(Guid id)
        {
            _id = id;
        }
    }
}

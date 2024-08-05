using ApplicationCore.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Applications.CQRS.Command.Create
{
    public class CreateCommand<TDomain, TReq> : IRequest
        where TDomain : Entity<Guid>
        where TReq : class
    {
        public TReq request { get; set; } = default!;

        public CreateCommand(TReq req)
        {
            request = req;
        }
    }
}

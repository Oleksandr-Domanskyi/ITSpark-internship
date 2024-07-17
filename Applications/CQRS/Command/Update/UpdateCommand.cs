using ApplicationCore.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Update
{
    public class UpdateCommand<TDomain, TReq> : IRequest
     where TDomain : Entity<Guid>
     where TReq : class
    {
        public TReq request { get; set; }
        public Guid Id { get; set; }
        public UpdateCommand(TReq req, Guid id)
        {
            request = req;
            Id = id;
        }
    }
}

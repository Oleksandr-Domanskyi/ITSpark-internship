using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Applications.CQRS.GeneratePDF
{
    public class GenerateProductListPDFQuery : IRequest<byte[]>
    {

    }
}
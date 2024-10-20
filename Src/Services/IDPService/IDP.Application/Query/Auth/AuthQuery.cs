using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Query.Auth
{
    public class AuthQuery : IRequest<bool>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}

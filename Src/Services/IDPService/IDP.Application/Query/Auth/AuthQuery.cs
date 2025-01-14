using Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Query.Auth
{
    public class AuthQuery : IRequest<JsonWebToken>
    {
        public required string MobileNumber { get; set; }
        public required int OptCode { get; set; }
    }
}

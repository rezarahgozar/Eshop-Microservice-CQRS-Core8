using Auth;
using IDP.Application.Query.Auth;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Handler.Query
{
    public class AuthHandler : IRequestHandler<AuthQuery, JsonWebToken>
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IOtpRedisRepository _otpRedisRepository;
        private readonly IUserQueryRepository _userQueryRepository;

        public AuthHandler(IJwtHandler jwtHandler, IOtpRedisRepository otpRedisRepository, IUserQueryRepository userQueryRepository)
        {
            _jwtHandler = jwtHandler;
            _otpRedisRepository = otpRedisRepository;
            _userQueryRepository = userQueryRepository;
        }
        public async Task<JsonWebToken> Handle(AuthQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _otpRedisRepository.GetDataAsync(request.MobileNumber);
                if (res == null) return null;
                if (res.OtpCode == request.OptCode)
                {
                    var user = await _userQueryRepository.GetUserAsync(request.MobileNumber);
                    var token = _jwtHandler.Create(user.Id);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

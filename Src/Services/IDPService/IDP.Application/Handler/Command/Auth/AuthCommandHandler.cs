using AutoMapper;
using IDP.Application.Command.Auth;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Handler.Command.Auth
{
    public class AuthCommandHandler : IRequestHandler<AuthCommand, bool>
    {
        private readonly IOtpRedisRepository _otpRedisRepository;
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IMapper _mapper;

        public AuthCommandHandler(IOtpRedisRepository otpRedisRepository,
            IUserCommandRepository userCommandRepository,
            IUserQueryRepository userQueryRepository,
            IMapper mapper)
        {
            _otpRedisRepository = otpRedisRepository;
            _userCommandRepository = userCommandRepository;
            _userQueryRepository = userQueryRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userObj = _mapper.Map<IDP.Domain.Entities.User>(request);
                var user = await _userQueryRepository.GetUserAsync(request.MobileNumber);
                if (user == null)
                {
                    Random random = new Random();
                    var code = random.Next(1000, 10000);
                    // send notification
                    userObj.UserName = request.MobileNumber;
                    var res = await _userCommandRepository.InsertAsync(userObj);
                    await _otpRedisRepository.InsertAsync(new Domain.DTO.Otp { UserName = userObj.MobileNumber, OtpCode = code, IsUse = false });

                }
                else
                {
                    Random random = new Random();
                    var code = random.Next(1000, 10000);
                    // send notification
                    userObj.UserName = request.MobileNumber;
                    await _otpRedisRepository.InsertAsync(new Domain.DTO.Otp { UserName = user.MobileNumber, OtpCode = code, IsUse = false });
                }
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
    }
}

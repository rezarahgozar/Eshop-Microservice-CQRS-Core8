using AutoMapper;
//using DotNetCore.CAP;
using IDP.Application.Command.Auth;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using EventMessages.Events;

namespace IDP.Application.Handler.Command.Auth
{
    public class AuthCommandHandler : IRequestHandler<AuthCommand, bool>
    {
        private readonly IOtpRedisRepository _otpRedisRepository;
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        //private readonly ICapPublisher _capPublisher;
        private readonly IMapper _mapper;

        public AuthCommandHandler(IOtpRedisRepository otpRedisRepository,
            IUserCommandRepository userCommandRepository,
            IUserQueryRepository userQueryRepository,
            //ICapPublisher capPublisher,
            IPublishEndpoint publishEndpoint,
            IMapper mapper)
        {
            _otpRedisRepository = otpRedisRepository;
            _userCommandRepository = userCommandRepository;
            _userQueryRepository = userQueryRepository;
            _publishEndpoint = publishEndpoint;
            //_capPublisher = capPublisher;
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
                    // CAP
                    //await _capPublisher.PublishAsync<AuthCommand>("otpevent", new AuthCommand 
                    //{
                    //    MobileNumber = request.MobileNumber,
                    //});


                    // MassTransit
                    await _publishEndpoint.Publish<OtpEvent>(new OtpEvent
                    {
                        CreateDate = DateTime.UtcNow,
                        MobileNumber = userObj.MobileNumber,
                        OtpCode = code.ToString(),
                    });

                    userObj.UserName = request.MobileNumber;
                    await _userCommandRepository.SaveChangesAsync();
                    var res = await _userCommandRepository.InsertAsync(userObj);
                    await _otpRedisRepository.InsertAsync(new Domain.DTO.Otp { UserName = userObj.MobileNumber, OtpCode = code, IsUse = false });

                }
                else
                {
                    Random random = new Random();
                    var code = random.Next(1000, 10000);
                    // send notification
                    // CAP
                    //await _capPublisher.PublishAsync<AuthCommand>("otpevent", new AuthCommand
                    //{
                    //    MobileNumber = request.MobileNumber,
                    //});

                    userObj.UserName = request.MobileNumber;
                    await _userCommandRepository.SaveChangesAsync();
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

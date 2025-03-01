﻿using Asp.Versioning;
using IDP.Application.Command.Auth;
using IDP.Application.Query.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDP.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthQuery authQuery)
        {
            var res = await _mediator.Send(authQuery);
            return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authQuery"></param>
        /// <returns></returns>
        [HttpPost("RegisterAndSendOtp")]
        public async Task<IActionResult> RegisterAndSendOtp([FromBody] AuthCommand authCommand)
        {
            var res = await _mediator.Send(authCommand);
            return Ok(res);
        }
    }
}

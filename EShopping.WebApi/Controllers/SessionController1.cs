using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EShopping.Data.Contracts;
using EShopping.Dtos;
using EShopping.Models;
using EShopping.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopping.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        private IUsersRepository _UsersRepository;
        private IMapper _mapper;
        private TokenService _tokenService;

        public SessionController(IUsersRepository UsersRepository,
                                IMapper mapper,
                                TokenService tokenService)
        {
            _UsersRepository = UsersRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        //POST: api/session/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> PostLogin(LoginModelDto UserLogin)
        {
            var loginUserData = _mapper.Map<User>(UserLogin);

            var validationResult = await _UsersRepository.ValidateLogin(loginUserData);
            if (!validationResult.result)
            {
                return BadRequest("Invalid User/Password");
            }
            return _tokenService.GenerateToken(validationResult.user);

        }

    }
}

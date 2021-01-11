using AutoMapper;
using EShopping.Data.Contracts;
using EShopping.Dtos;
using EShopping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShopping.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IUsersRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUsersRepository _usersRepository, IMapper mapper)
        {
            this._userRepository = _usersRepository;
            this._mapper = mapper;
        }

        //// GET: api/users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserListDto>>> Get()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return _mapper.Map<List<UserListDto>>(users);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserListDto>> Get(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return _mapper.Map<UserListDto>(user);
        }

        // POST: api/users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserListDto>> Post(UserRegisterDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);

                var newUser = await _userRepository.Add(user);
                if (newUser == null)
                {
                    return BadRequest();
                }

                var newUserDto = _mapper.Map<UserListDto>(newUser);
                return CreatedAtAction(nameof(Post), new { id = newUserDto.Id }, newUserDto);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserListDto>> Put(int id, [FromBody]UserUpdateDto userDto)
        {
            if (userDto == null)
                return NotFound();

            var user = _mapper.Map<User>(userDto);
            var result = await _userRepository.Update(user);
            if (!result)
                return BadRequest();

            return _mapper.Map<UserListDto>(user);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _userRepository.Delete(id);
                if (!resultado)
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception excepcion)
            {
                return BadRequest();
            }
        }

        // POST: api/users/changepassword
        [HttpPost]
        [Route("changepassword")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostChangePassword(LoginModelDto userPasswordDto)
        {
            try
            {
                var user = _mapper.Map<User>(userPasswordDto);
                var resultado = await _userRepository.ChangePassword(user);
                if (!resultado)
                {
                    return BadRequest();
                }
                return NoContent();                

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: api/users/changeprofile
        [HttpPost]
        [Route("changeprofile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostChangeProfile(UserProfileDto userProfileDto)
        {
            try
            {
                var user = _mapper.Map<User>(userProfileDto);

                var result = await _userRepository.ChangeProfile(user);
                if (!result)
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: api/users/validateuser
        [HttpPost]
        [Route("validateuser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserRegisterDto>> PostValidateUser(LoginModelDto userPasswordDto)
        {
            try
            {
                var user = _mapper.Map<User>(userPasswordDto);

                var resultado = await _userRepository.ValidatePassword(user);
                if (!resultado)
                {
                    return BadRequest();
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
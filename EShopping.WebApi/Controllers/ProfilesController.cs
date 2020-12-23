using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EShopping.Data.Contracts;
using EShopping.Dtos;
using EShopping.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EShopping.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private IGenericRepository<Models.Profile> _profilesRepository;
        private readonly ILogger<ProfilesController> _logger;
        private readonly IMapper _mapper;

        public ProfilesController(IGenericRepository<Models.Profile> ProfilesRepository, 
            ILogger<ProfilesController> logger,
            IMapper mapper)
        {
            this._profilesRepository = ProfilesRepository;
            this._logger = logger;
            this._mapper = mapper;
        }

        // GET: api/Profiles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> Get()
        {
            try
            {
                var profiles= await _profilesRepository.GetAllAsync();
                return _mapper.Map<List<ProfileDto>>(profiles);
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error in {nameof(Get)}: " + excepcion.Message);
                return BadRequest();
            }
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProfileDto>> Get(int id)
        {
            var profile = await _profilesRepository.GetAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return _mapper.Map<ProfileDto>(profile);
        }

        // POST: api/Profiles
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProfileDto>> Post(ProfileDto ProfileDto)
        {
            try
            {
                var Profile = _mapper.Map<Models.Profile>(ProfileDto);

                var newProfile = await _profilesRepository.Add(Profile);
                if (newProfile == null)
                {
                    return BadRequest();
                }

                var nuevoProfileDto = _mapper.Map<ProfileDto>(newProfile);
                return CreatedAtAction(nameof(Post), new { id = nuevoProfileDto.Id }, nuevoProfileDto);

            }
            catch (Exception excepcion)            
            {
                _logger.LogError($"Error in {nameof(Post)}: " + excepcion.Message);
                return BadRequest();
            }
        }

        // PUT: api/Profiles/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProfileDto>> Put(int id, [FromBody]ProfileDto ProfileDto)
        {
            if (ProfileDto == null)
                return NotFound();


            var Profile = _mapper.Map<Models.Profile>(ProfileDto);
            var resultado = await _profilesRepository.Update(Profile);
            if (!resultado)
                return BadRequest();

            return ProfileDto;
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _profilesRepository.Delete(id);
                if (!result)
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error in {nameof(Delete)}: " + excepcion.Message);
                return BadRequest();
            }
        }


    }
}
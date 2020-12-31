using AutoMapper;
using EShopping.Data.Contracts;
using EShopping.Dtos;
using EShopping.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShopping.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersRepository ordersRepository, 
            ILogger<OrdersController> logger,IMapper mapper)
        {
            this._ordersRepository = ordersRepository;
            this._mapper = mapper;
        }

        //// GET: api/orders
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            try
            {
                var orders= await _ordersRepository.GetAllAsync();
                return _mapper.Map<List<OrderDto>>(orders);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        //// GET: api/orders/details
        [HttpGet]
        [Route("details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersWithDetail()
        {
            try
            {
                var orders = await _ordersRepository.GetAllWithDetailsAsync();
                return _mapper.Map<List<OrderDto>>(orders);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            var order = await _ordersRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return _mapper.Map<OrderDto>(order);
        }


        // GET: api/orders/5/details
        [HttpGet("{id}/details")]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderWithDetails(int id)
        {
            var order = await _ordersRepository.GetWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return _mapper.Map<OrderDto>(order);
        }

        // POST: api/orders
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> Post(OrderDto OrderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(OrderDto);

                var newOrder = await _ordersRepository.Add(order);
                if (newOrder == null)
                {
                    return BadRequest();
                }

                var nuevaOrderDto = _mapper.Map<OrderDto>(newOrder);
                return CreatedAtAction(nameof(Post), new { id = nuevaOrderDto.Id }, nuevaOrderDto);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        
        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _ordersRepository.Delete(id);
                if (!result)
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

    }
}
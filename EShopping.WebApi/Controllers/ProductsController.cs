using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShopping.Data;
using EShopping.Models;
using EShopping.Data.Contracts;
using AutoMapper;
using EShopping.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EShopping.WebApi.Controllers
{
    [Authorize(Roles = "Admin,Seller")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductsRepository _productsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsRepository productsRepository, 
            IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _productsRepository = productsRepository;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            try
            {
                var products = await _productsRepository.GetProductsAsync();
                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Get)}: ${ex.Message}");
                return BadRequest();
            }
        }

        // PUT: api/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            try
            {
                var product = await _productsRepository.GetProductAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: api/Products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> Post(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);

                var newProduct = await _productsRepository.Add(product);
                if (newProduct == null)
                {
                    return BadRequest();
                }

                var newProductDto = _mapper.Map<ProductDto>(newProduct);
                return CreatedAtAction(nameof(Post), new { id = newProductDto.Id }, newProductDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Post)}:${ex.Message}");
                return BadRequest();
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ProductDto>> Put(int Id, [FromBody]ProductDto productDto)
        {
            try
            {
                if (productDto == null) 
                {
                    return NotFound();
                }

                var product = _mapper.Map<Product>(productDto);

                var result = await _productsRepository.Update(product);
                if (!result) 
                {
                    return BadRequest();
                }

                return productDto;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var result = await _productsRepository.Delete(id);
                if (!result) 
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}: ${ex.Message}");
                return BadRequest();
            }
        }


        //// GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    return await _context.Products.ToListAsync();
        //}

        //// GET: api/Products/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product;
        //}

        //// PUT: api/Products/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(int id, Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Products
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        //}

        //// DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Product>> DeleteProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return product;
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}

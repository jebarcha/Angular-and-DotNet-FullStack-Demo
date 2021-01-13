using EShopping.Data.Contracts;
using EShopping.Models;
using EShopping.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopping.Data.Repositories
{

    public class ProductsRepository : IProductsRepository
    {
        private EShoppingDbContext _context;
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(EShoppingDbContext contexto, ILogger<ProductsRepository> logger)
        {
            _context = contexto;
            this._logger = logger;
        }
        public async Task<bool> Update(Product product)
        {
            var productDb = await GetProductAsync(product.Id);
            productDb.Name = product.Name;
            productDb.Price = product.Price;

            //_contexto.Products.Attach(product);
            //_contexto.Entry(producto).State = EntityState.Modified;
            try
            {
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Update)}: {ex.Message}");
            }
            return false;
        }

        public async Task<Product> Add(Product product)
        {
            product.Status = ProductStatus.Active;
            product.DateAdded = DateTime.UtcNow;

            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Add)}: {ex.Message}");
            }

            return product;
        }

        public async Task<bool> Delete(int id)
        {
            //Se realiza una eliminación suave, solamente inactivamos el producto

            var product = await _context.Products
                                .SingleOrDefaultAsync(c => c.Id == id);

            product.Status = ProductStatus.Inactive;
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}: {ex.Message}");
            }
            return false;

        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _context.Products
                               .SingleOrDefaultAsync(c => c.Id == id && c.Status == ProductStatus.Active);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Where(u => u.Status == ProductStatus.Active)
                .OrderBy(u => u.Name)
                   .ToListAsync();
        }

        public async Task<(int totalRecords, IEnumerable<Product> records)> GetPagesProductsAsync(int currentPage, int recordsPerPage)
        {
            var totalRecords = await _context.Products
                .Where(u => u.Status == ProductStatus.Active)
                .CountAsync();

            var records = await _context.Products
                                .Where(u => u.Status == ProductStatus.Active)
                                .Skip((currentPage - 1) * recordsPerPage)
                                .Take(recordsPerPage)
                                .ToListAsync();
            return (totalRecords, records);
        }
    }

}

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
        private EShoppingDbContext _contexto;
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(EShoppingDbContext contexto, ILogger<ProductsRepository> logger)
        {
            _contexto = contexto;
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
                return await _contexto.SaveChangesAsync() > 0 ? true : false;
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

            _contexto.Products.Add(product);
            try
            {
                await _contexto.SaveChangesAsync();
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

            var product = await _contexto.Products
                                .SingleOrDefaultAsync(c => c.Id == id);

            product.Status = ProductStatus.Inactive;
            _contexto.Products.Attach(product);
            _contexto.Entry(product).State = EntityState.Modified;

            try
            {
                return (await _contexto.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}: {ex.Message}");
            }
            return false;

        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _contexto.Products
                               .SingleOrDefaultAsync(c => c.Id == id && c.Status == ProductStatus.Active);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _contexto.Products
                .Where(u => u.Status == ProductStatus.Active)
                .OrderBy(u => u.Name)
                   .ToListAsync();
        }
    }

}

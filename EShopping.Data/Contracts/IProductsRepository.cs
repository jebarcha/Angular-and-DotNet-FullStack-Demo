using EShopping.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShopping.Data.Contracts
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> Add(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(int id);
    }

}

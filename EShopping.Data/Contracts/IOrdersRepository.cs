using EShopping.Data.Contracts;
using EShopping.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShopping.Data.Contracts
{
    public interface IOrdersRepository: IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task<Order> GetWithDetailsAsync(int id);
    }
}

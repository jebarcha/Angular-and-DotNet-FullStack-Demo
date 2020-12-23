using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShopping.Data.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
    }
}

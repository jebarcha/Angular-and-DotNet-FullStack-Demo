using EShopping.Data;
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
    public class OrdersRepository : IOrdersRepository
    {
        private readonly EShoppingDbContext _context;
        private readonly ILogger<ProfilesRepository> _logger;
        private DbSet<Order> _dbSet;

        public OrdersRepository(EShoppingDbContext contexto, ILogger<ProfilesRepository> logger)
        {
            this._context = contexto;
            this._logger = logger;
            this._dbSet = _context.Set<Order>();
        }

        public async Task<bool> Update(Order entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Update)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<Order> Add(Order entity)
        {
            entity.OrderStatus = OrderStatus.Active;
            entity.DateAdded = DateTime.UtcNow;            
            _dbSet.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Add)}: " + excepcion.Message);
                return null;
            }
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(u => u.Id == id);
            entity.OrderStatus = OrderStatus.Inactive;            
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Delete)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<Order> GetAsync(int id)
        {
            return await _dbSet.Include(order=> order.User)                    
                                .SingleOrDefaultAsync(c => c.Id == id 
                                && c.OrderStatus == OrderStatus.Active);
        }

        public async Task<Order> GetWithDetailsAsync(int id)
        {
            return await _dbSet.Include(order => order.User)
                    .Include(order => order.OrderDetail)
                        .ThenInclude(orderDetail => orderDetail.Product)
                    .SingleOrDefaultAsync(c => c.Id == id
                    && c.OrderStatus == OrderStatus.Active);

        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet.Where(u=>u.OrderStatus== OrderStatus.Active)
                                .Include(order => order.User)                                                               
                                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _dbSet.Where(u => u.OrderStatus == OrderStatus.Active)
                                .Include(order => order.User)
                                .Include(order => order.OrderDetail)
                                    .ThenInclude(orderDetail => orderDetail.Product)
                                .ToListAsync();
        }

    }
}

using EShopping.Data;
using EShopping.Data.Contracts;
using EShopping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShopping.Data.Repositories
{
    public class ProfilesRepository : IGenericRepository<Profile>
    {
        private readonly EShoppingDbContext _context;
        private readonly ILogger<ProfilesRepository> _logger;
        private DbSet<Profile> _dbSet;

        public ProfilesRepository(EShoppingDbContext contexto, ILogger<ProfilesRepository> logger)
        {
            this._context = contexto;
            this._logger = logger;
            this._dbSet = _context.Set<Profile>();
        }
        public async Task<bool> Update(Profile entity)
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

        public async Task<Profile> Add(Profile entity)
        {
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
            _dbSet.Remove(entity);
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

        public async Task<Profile> GetAsync(int id)
        {
            return await _dbSet.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}

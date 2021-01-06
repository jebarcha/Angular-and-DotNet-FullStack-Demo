using EShopping.Data.Contracts;
using EShopping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using EShopping.Models.Enum;
using System.Linq;

namespace EShopping.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly EShoppingDbContext _context;
        private readonly ILogger<UsersRepository> _logger;        
        private readonly IPasswordHasher<User> _passwordHasher;
        private DbSet<User> _dbSet;
        public UsersRepository(EShoppingDbContext contexto, 
            ILogger<UsersRepository> logger,
            IPasswordHasher<User> passwordHasher)
        {
            this._context = contexto;
            this._logger = logger;
            this._passwordHasher = passwordHasher;
            this._dbSet = _context.Set<User>();
        }
        public async Task<bool> Update(User entity)
        {
            var UserDb = await _dbSet.FirstOrDefaultAsync(u => u.Id == entity.Id);

            if (UserDb == null)
            {
                _logger.LogError($"Error en {nameof(Update)}: No existe el User con Id: {entity.Id}");
                return false;
            }

            UserDb.Name = entity.Name;
            UserDb.LastName = entity.LastName;
            UserDb.Email = entity.Email;            
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

        public async Task<User> Add(User entity)
        {
            entity.Status = UserStatus.Active;
            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);
            _dbSet.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Add)}: " + excepcion.Message);
            }
            return entity;
        }

        public async Task<bool> ChangePassword(User User)
        {            
            var UserBd = await _dbSet.FirstOrDefaultAsync(u => u.Id == User.Id);
            UserBd.Password = _passwordHasher.HashPassword(UserBd, User.Password);
            try
            {
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(ChangePassword)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<bool> ChangeProfile(User User)
        {
            var UserBd = await _dbSet.FirstOrDefaultAsync(u => u.Id == User.Id);
            UserBd.ProfileId = User.ProfileId;
            try
            {
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(ChangeProfile)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(u => u.Id == id);
            entity.Status = UserStatus.Inactive;
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

        public async Task<User> GetAsync(int id)
        {
            return await _dbSet.Include(User => User.Profile)
                                .SingleOrDefaultAsync(c => c.Id == id && c.Status==UserStatus.Active);
        }

     

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.Include(User => User.Profile)
                                .Where(u=>u.Status==UserStatus.Active)
                                .ToListAsync();
        }

        public async Task<bool> ValidatePassword(User User)
        {
            var UserBd = await _dbSet.FirstOrDefaultAsync(u => u.Id == User.Id);                        
            try
            {
                var resultado = _passwordHasher.VerifyHashedPassword(UserBd, UserBd.Password, User.Password);
                return resultado == PasswordVerificationResult.Success ? true : false;                                
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(ValidatePassword)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<(bool result, User user)> ValidateLogin(User loginUser)
        {
            var userDb = await _dbSet
                                    .Include(u => u.Profile)
                                    .FirstOrDefaultAsync(u => u.Username == loginUser.Username);
            if (userDb != null)
            {
                try
                {
                    var result = _passwordHasher.VerifyHashedPassword(userDb, userDb.Password, loginUser.Password);
                    return (result == PasswordVerificationResult.Success ? true : false, userDb);
                }
                catch (Exception excepcion)
                {
                    _logger.LogError($"Error in {nameof(ValidateLogin)}: " + excepcion.Message);
                }
            }
            return (false, null);
        }

    }
}

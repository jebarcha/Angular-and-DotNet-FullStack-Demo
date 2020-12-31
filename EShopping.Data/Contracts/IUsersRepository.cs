using EShopping.Models;
using System.Threading.Tasks;

namespace EShopping.Data.Contracts
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<bool> ChangePassword(User user);
        Task<bool> ChangeProfile(User user);
        Task<bool> ValidatePassword(User user);
    }
}

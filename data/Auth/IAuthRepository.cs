using System.Collections.Generic;
using System.Threading.Tasks;
using shop_giay_server.models;

namespace shop_giay_server.data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);

        Task<IEnumerable<User>> GetUser();

        Task<User> GetUserById(int id);

        Task<User> Update(User user);
    }
}
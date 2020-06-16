using System.Threading.Tasks;
using shop_giay_server.models;

namespace shop_giay_server.data.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<Customer> Login(string username, string password);
    }
}
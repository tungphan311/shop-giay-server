using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shop_giay_server.models;

namespace shop_giay_server.data.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext _context;
        public AuthenticationRepository(DataContext context)
        {
            _context = context;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public async Task<Customer> Login(string username, string password)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Username == username);

            if (customer == null) return null;

            if (!VerifyPassword(password, customer.PasswordHash, customer.PasswordSalt)) return null;

            return customer;
        }


    }
}
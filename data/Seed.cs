using Newtonsoft.Json;
using shop_giay_server.models;
using System.Collections.Generic;
using System.Linq;

namespace shop_giay_server.data
{
    public class Seed
    {
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public static void SeedShoes(DataContext context)
        {
            if (!context.Shoes.Any())
            {
                var shoesData = System.IO.File.ReadAllText("data/seeds/Shoes.json");

                var shoes = JsonConvert.DeserializeObject<List<Shoes>>(shoesData);

                foreach (var s in shoes)
                {
                    s.Rating = 0;
                    s.IsNew = true;
                    s.IsOnSale = false;

                    context.Shoes.Add(s);
                }

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("data/seeds/Users.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var u in users)
                {
                    byte[] passwordHash, passwordSalt;

                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    u.PasswordHash = passwordHash;
                    u.PasswordSalt = passwordSalt;
                    u.RoleId = 1;
                    u.DeleteFlag = false;

                    context.Users.Add(u);
                }

                context.SaveChanges();
            }
        }
    }
}
using Newtonsoft.Json;
using shop_giay_server.models;
using System;
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

        public static void SeedAll(DataContext context)
        {
            if (!context.Roles.Any())
            {
                var arr = new string[] { "admin", "staff" };
                foreach (var i in arr)
                {
                    var item = new Role();
                    item.Name = i;
                    item.DeleteFlag = false;
                    context.Add(item);
                }

                context.SaveChanges();
            }

            if (!context.Colors.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/Colors.json");

                var items = JsonConvert.DeserializeObject<List<Color>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.Colors.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.CustomerTypes.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/CustomerTypes.json");

                var items = JsonConvert.DeserializeObject<List<CustomerType>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.CustomerTypes.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.Genders.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/Gender.json");

                var items = JsonConvert.DeserializeObject<List<Gender>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.Genders.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.Providers.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/Providers.json");

                var items = JsonConvert.DeserializeObject<List<Provider>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.Providers.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.Sales.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/Sales.json");

                var items = JsonConvert.DeserializeObject<List<Sale>>(itemData);

                if (items != null)
                {
                    foreach (var i in items)
                    {
                        i.DeleteFlag = false;
                        context.Sales.Add(i);
                    }

                    context.SaveChanges();
                }
            }

            if (!context.ShoesBrands.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/ShoesBrands.json");

                var items = JsonConvert.DeserializeObject<List<ShoesBrand>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.ShoesBrands.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.ShoesTypes.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/ShoesTypes.json");

                var items = JsonConvert.DeserializeObject<List<ShoesType>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.ShoesTypes.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.Sizes.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/Sizes.json");

                var items = JsonConvert.DeserializeObject<List<Size>>(itemData);

                foreach (var i in items)
                {
                    i.DeleteFlag = false;
                    context.Sizes.Add(i);
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

            if (!context.Shoes.Any())
            {
                var shoesData = System.IO.File.ReadAllText("data/seeds/Shoes.json");

                var shoes = JsonConvert.DeserializeObject<List<Shoes>>(shoesData);

                foreach (var s in shoes)
                {
                    s.Rating = 0;
                    s.IsNew = true;
                    s.IsOnSale = false;
                    s.DeleteFlag = false;

                    context.Shoes.Add(s);
                }

                context.SaveChanges();
            }

            if (!context.ShoesImages.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/ShoesImages.json");

                var items = JsonConvert.DeserializeObject<List<ShoesImage>>(itemData);

                var shoes = context.Shoes.ToList();

                var colors = context.Colors.ToList();

                var random = new Random();

                foreach (var i in items)
                {
                    var colorId = colors[random.Next(0, colors.Count - 1)].Id;
                    var shoesId = shoes[random.Next(0, shoes.Count - 1)].Id;
                    i.ColorId = colorId;
                    i.ShoesId = shoesId;
                    i.DeleteFlag = false;
                    context.Add(i);
                }

                context.SaveChanges();
            }

            if (!context.Stocks.Any())
            {
                var shoes = context.Shoes.ToList();
                var colors = context.Colors.ToList();
                var sizes = context.Sizes.ToList();

                var random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    var stock = new Stock();
                    var colorId = colors[random.Next(0, colors.Count - 1)].Id;
                    var shoesId = shoes[random.Next(0, shoes.Count - 1)].Id;
                    var sizeId = shoes[random.Next(0, sizes.Count - 1)].Id;
                    var numb = random.Next(0, 10);
                    stock.ColorId = colorId;
                    stock.ShoesId = shoesId;
                    stock.SizeId = sizeId;
                    stock.Instock = numb;
                    stock.DeleteFlag = false;
                    context.Add(stock);
                }

                context.SaveChanges();
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
                    s.DeleteFlag = false;

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
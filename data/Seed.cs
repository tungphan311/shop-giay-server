using Newtonsoft.Json;
using shop_giay_server.models;
using System;
using System.Collections.Generic;
using System.Linq;
using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Collections.Generic;
using shop_giay_server.data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;

namespace shop_giay_server.data
{
    public class Seed
    {
        static public List<string> Brand1 = new List<string>()
        {
            "Nike",
            "Adidas",
            "Air Jordan",
            "Converse",
            "Supreme",
            "Vans",
            "Yeezy"
        };
        static public List<string> _ShoesImages = new List<string>() {
            "https://assets.adidas.com/images/w_600,f_auto,q_auto/160be65a2069403f9df9a8270018e424_9366/NMD_R1_Shoes_Black_B79758_01_standard.jpg",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/nike-women-s-air-max-200-se-cj0630-600-1_600x.png?v=1593233974",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/air-jordan-xi-11-retro-low-av2187-160-1_600x.png?v=1593402702",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/air-jordan-1-mid-554724-092-1_600x.png?v=1593402583",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/air-jordan-1-retro-low-553558-130-1_600x.png?v=1593402343",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_be25f072-1707-4543-bc1a-aa90f1fedac5_600x.png?v=1588830762",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_80be7d31-344a-4b13-a915-184dbaa09487_600x.png?v=1588951046",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_417a99b2-4b08-486a-af5e-95c43ff2203b_600x.png?v=1590906259",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_417a99b2-4b08-486a-af5e-95c43ff2203b_600x.png?v=1590906259",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_bc2f1b6f-f96e-42b6-b771-028ae9e616db_600x.png?v=1588830252",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/converse-chuck-taylor-all-star-high-567991c-1_600x.png?v=1593384493",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/converse-chuck-taylor-all-star-low-567992c-1_a42a8497-36af-4b7d-a89f-00c0fb3a7f7a_600x.png?v=1591828025",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_35e318b9-4e71-43bd-bdf1-1318a05599fa_600x.png?v=1591189630",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/converse-chuck-taylor-all-star-lugged-567681c-1_600x.png?v=1593461792",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_d8576316-2120-43ce-92c7-e02514c8b8f3_600x.png?v=1591189865",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/vans-x-supreme-sk8-hi-pro-fuck-the-world-vn0a45jdsy4-1_e016fc59-6702-45b5-92c8-73fdc0404044_600x.png?v=1593395190",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_1d6ef136-97fb-4b41-8684-70b79f2825e5_600x.png?v=1589999190",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_780cd9a9-1ba7-4177-8bf3-ba48fe102e80_600x.png?v=1589907671",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_960671dc-47b2-4030-b4af-ac2a423d6672_600x.png?v=1589998376",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_1200fa46-01f7-45c3-bd31-0d00abfa06a3_600x.png?v=1590087516",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/nike-air-max-270-react-se-ct1265-300-1_600x.png?v=1592497780",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/nike-waffle-racer-cn8116-100-1_42f2aabd-bee9-4cea-9c97-9ff5e3c05123_600x.png?v=1592593388",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_dfd3e406-8a3e-4b2a-aefd-6cc8e948afd3_600x.png?v=1591507780",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/nike-acg-moc-3-0-ct3302-400-1_600x.png?v=1592456746",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_9d54eac8-15ff-4343-9735-f75878f8a165_600x.png?v=1588900839",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_71c622a0-3ca8-4364-a829-c48c33f250d5_600x.png?v=1588900782",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_000097d5-dd1a-4805-aaf7-40311e34a0ba_600x.png?v=1588788446",
"https://cdn.shopify.com/s/files/1/0324/6781/2487/products/shopify-full-image-2000x2000_bc2f1b6f-f96e-42b6-b771-028ae9e616db_600x.png?v=1588830252"
        };

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static void Run(DataContext _context)
        {
            try
            {
                var rand = new Random();
                var listShoes = _context.Shoes.Include(c => c.ShoesImages).Include(c => c.Stocks);
                foreach (var shoes in listShoes)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        var image = new ShoesImage()
                        {
                            ImagePath = _ShoesImages[rand.Next(0, _ShoesImages.Count() - 1)],
                            ColorId = 1
                        };
                        shoes.ShoesImages.Add(image);
                    }


                    var sizeId = rand.Next(4, 12);
                    var quantity = rand.Next(20, 88);
                    shoes.Stocks.Add(new Stock()
                    {
                        SizeId = sizeId,
                        Instock = quantity,
                        ShoesId = shoes.Id,
                        ColorId = 1
                    });

                    shoes.Stocks.Add(new Stock()
                    {
                        SizeId = sizeId == 12 ? sizeId - 1 : sizeId + 1,
                        Instock = quantity,
                        ShoesId = shoes.Id,
                        ColorId = 1
                    });
                }

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("error");
            }

            Console.WriteLine("Done");

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

            // if (!context.ShoesBrands.Any())
            // {
            //     var itemData = System.IO.File.ReadAllText("data/seeds/ShoesBrands.json");

            //     var items = JsonConvert.DeserializeObject<List<ShoesBrand>>(itemData);

            //     foreach (var i in items)
            //     {
            //         i.DeleteFlag = false;
            //         context.ShoesBrands.Add(i);
            //     }

            //     context.SaveChanges();
            // }

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

            // if (!context.Sizes.Any())
            // {
            //     var itemData = System.IO.File.ReadAllText("data/seeds/Sizes.json");

            //     var items = JsonConvert.DeserializeObject<List<Size>>(itemData);

            //     foreach (var i in items)
            //     {
            //         i.DeleteFlag = false;
            //         context.Sizes.Add(i);
            //     }

            //     context.SaveChanges();
            // }

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

            // if (!context.Shoes.Any())
            // {
            //     var shoesData = System.IO.File.ReadAllText("data/seeds/Shoes.json");

            //     var shoes = JsonConvert.DeserializeObject<List<Shoes>>(shoesData);

            //     foreach (var s in shoes)
            //     {
            //         s.Rating = 0;
            //         s.IsNew = true;
            //         s.IsOnSale = false;
            //         s.DeleteFlag = false;

            //         context.Shoes.Add(s);
            //     }

            //     context.SaveChanges();
            // }

            // if (!context.ShoesImages.Any())
            // {
            //     var itemData = System.IO.File.ReadAllText("data/seeds/ShoesImages.json");

            //     var items = JsonConvert.DeserializeObject<List<ShoesImage>>(itemData);

            //     var shoes = context.Shoes.ToList();

            //     var colors = context.Colors.ToList();

            //     var random = new Random();

            //     foreach (var i in items)
            //     {
            //         var colorId = colors[random.Next(0, colors.Count - 1)].Id;
            //         var shoesId = shoes[random.Next(0, shoes.Count - 1)].Id;
            //         i.ColorId = colorId;
            //         i.ShoesId = shoesId;
            //         i.DeleteFlag = false;
            //         context.Add(i);
            //     }

            //     context.SaveChanges();
            // }

            // if (!context.Stocks.Any())
            // {
            //     var shoes = context.Shoes.ToList();
            //     var colors = context.Colors.ToList();
            //     var sizes = context.Sizes.ToList();

            //     var random = new Random();

            //     for (int i = 0; i < 10; i++)
            //     {
            //         var stock = new Stock();
            //         var colorId = colors[random.Next(0, colors.Count - 1)].Id;
            //         var shoesId = shoes[random.Next(0, shoes.Count - 1)].Id;
            //         var sizeId = shoes[random.Next(0, sizes.Count - 1)].Id;
            //         var numb = random.Next(0, 10);
            //         stock.ColorId = colorId;
            //         stock.ShoesId = shoesId;
            //         stock.SizeId = sizeId;
            //         stock.Instock = numb;
            //         stock.DeleteFlag = false;
            //         context.Add(stock);
            //     }

            //     context.SaveChanges();
            // }

            if (!context.Customers.Any())
            {
                var customerData = System.IO.File.ReadAllText("data/seeds/Customers.json");

                var customers = JsonConvert.DeserializeObject<List<Customer>>(customerData);

                foreach (var c in customers)
                {
                    byte[] passwordHash, passwordSalt;

                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    c.Point = 0;
                    c.DateOfBirth = new DateTime(1998, 1, 31);
                    c.PasswordHash = passwordHash;
                    c.PasswordSalt = passwordSalt;
                    c.CustomerTypeId = 2;

                    context.Add(c);
                }

                context.SaveChanges();
            }

            if (!context.Addresses.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/Addresses.json");

                var items = JsonConvert.DeserializeObject<List<Address>>(itemData);

                foreach (var item in items)
                {
                    item.DeleteFlag = false;
                    context.Addresses.Add(item);
                }

                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var orderData = System.IO.File.ReadAllText("data/seeds/Orders.json");

                var orders = JsonConvert.DeserializeObject<List<Order>>(orderData);

                foreach (var order in orders)
                {
                    order.DeleteFlag = false;
                    order.OrderDate = new DateTime(2020, 6, 20, 10, 20, 15);
                    order.Status = 1;
                    order.ConfirmDate = null;
                    order.DeliveryDate = null;

                    context.Orders.Add(order);
                }

                context.SaveChanges();
            }

            if (!context.OrderItems.Any())
            {
                var itemData = System.IO.File.ReadAllText("data/seeds/OrderItems.json");

                var items = JsonConvert.DeserializeObject<List<OrderItem>>(itemData);

                foreach (var item in items)
                {
                    item.DeleteFlag = false;

                    context.OrderItems.Add(item);
                }

                context.SaveChanges();
            }
        }
    }
}



/*
    static public List<(string code, string name, string desc, int style, int bra, int gender, float price)> Sizes =
            new List<(string code, string name, string desc, int style, int bra, int gender, float price)>()
            {
                ("SH0001","Adidas Yeezy Boost 350 V2 (Earth)","Developed by Kanye West, the Adidas Yeezy Boost 350 V2 is the latest version of the popular 350 model. Constructed with a Primeknit upper, they also feature internal toebox reinforcement, see-through panel, full-length Boost technology, rope laces, and a two-toned rubber outsole. This specific colorway is a limited release to the US region.",1,2,1,(float)129.99),
("SH0002","Nike Women's Air Max 200 SE","With a design inspired by the Nike Air Max 180, the Nike Air Max 200 is a new design with a familiar look. They are constructed with a mesh upper, feature suede overlays, visible Max Air unit, rope laces, and a heel pull tab.",2,1,2,(float)234.95),
("SH0003","Air Jordan XI (11) Retro Low (Concord Bred)","The Air Jordan XI has become the most sought-after Air Jordan design ever created. This popular model was first released in 1995 when Jordan laced them up and led the Chicago Bulls to an all-time NBA best 72-win season. The stylish Air Jordan XI Low features a Cordura upper, carbon fiber spring plate, wraparound patent leather detail, and a translucent sole. Air Jordan XI (11) Retro Low (Concord Bred) av2187-160 is released in June 2020.",1,3,1,(float)144.95),
("SH0004","Air Jordan 1 Mid (Smoke Grey)","Simply put, the Air Jordan 1 is the sneaker that started it all. Michael Jordan's first signature model was released in 1985, and is the Air Jordan model that sports a Nike Swoosh. The Air Jordan 1 violated the NBA's uniform policy, which led to Jordan being fined $5,000 a game, and became a topic of a popular Nike commercial. The Air Jordan 1 is still popular today, and has been released in more colorways than any other Air Jordan model.",1,3,1,(float)125.95),
("SH0005","Air Jordan 1 Retro Low","Simply put, the Air Jordan 1 is the sneaker that started it all. Michael Jordan's first signature model was released in 1985, and is the Air Jordan model that sports a Nike Swoosh. The Air Jordan 1 violated the NBA's uniform policy, which led to Jordan being fined $5,000 a game, and became a topic of a popular Nike commercial. The Air Jordan 1 is still popular today, and has been released in more colorways than any other Air Jordan model.",3,3,1,(float)89.99),
("SH0006","Adidas Torsion X","The Adidas Torsion X is equipped with Torsion X, a support system that allows your foot to move naturally. They also have a breathable textile upper, padded collar/tongue, energy-returning Boost cushioning, and a rubber outsole.",1,2,1,(float)179.99),
("SH0007","Adidas SL 72","The Adidas SL72 debuted in 1972 to coincide with the '72 Olympic Games. Marketed as an all-around trainer, they originally featured a nylon/suede upper, classic design, Adidas Trefoil logo, and a rubber outsole. Today are available in numerous materials/colors and add a great retro look to any outfit.",2,2,1,(float)89.99),
("SH0008","Adidas Stan Smith W (Fiorucci Stan)","One of the most recognizable tennis sneakers ever created, the Adidas Stan Smith has a timeless athletic design. Named the \"Stan Smith\" in 1971 (for tennis professional Stan Smith), they're constructed with a smooth leather upper, perforated 3-Stripes, and \"STAN SMITH\" on the tongue and heel.",1,2,1,(float)109.99),
("SH0009","Adidas Stan Smith Women","One of the most recognizable tennis sneakers ever created, the Adidas Stan Smith has a timeless athletic design. Named the \"Stan Smith\" in 1971 (for tennis professional Stan Smith), they're constructed with a smooth leather upper, perforated 3-Stripes, and \"STAN SMITH\" on the tongue and heel.",3,2,2,(float)89.99),
("SH0010","Adidas Ozweego W","Inspired by Ozweego heritage, the latest version of the Adidas Ozweego fuses new and old design elements. They have a mesh upper with suede overlays, Adiprene cushioning, rope laces, and a rubber outsole.",1,2,2,(float)109.99),
("SH0011","Converse Women's Chuck Taylor All Star High (Friendship Bracelet)","One of the most stylish, timeless, and versatile sneakers ever created, the iconic Converse Chuck Taylor All Star. Originally released as the All Star in 1917, they added the Chuck Taylor name after the popular basketball player endorsed the model.This \"70\" version incorporates vintage details to pay homage to the original Chuck Taylor All Star, including higher rubber siding and an OrthoLite cushioned footbed.",1,4,1,(float)74.99),
("SH0012","Converse Women's Chuck Taylor All Star Low (Friendship Bracelet)","One of the most stylish, timeless, and versatile sneakers ever created, the iconic Converse Chuck Taylor All Star. Originally released as the All Star in 1917, the Chuck Taylor name was added after the popular basketball player endorsed the model. Currently, the Converse Chuck Taylor All Star is still an extremely popular sneaker and worn across the globe by men, women, boys and girls. They are also now available in an array of colors, materials and modified designs.",2,4,1,(float)69.99),
("SH0013","Converse Women's Chuck Taylor All Star High (VLTG)","One of the most stylish, timeless, and versatile sneakers ever created, the iconic Converse Chuck Taylor All Star. Originally released as the All Star in 1917, they added the Chuck Taylor name after the popular basketball player endorsed the model.This \"70\" version incorporates vintage details to pay homage to the original Chuck Taylor All Star, including higher rubber siding and an OrthoLite cushioned footbed.",1,4,2,(float)69.99),
("SH0014","Converse Women's Chuck Taylor All Star Lugged","One of the most stylish, timeless, and versatile sneakers ever created, the iconic Converse Chuck Taylor All Star. Originally released as the All Star in 1917, the Chuck Taylor name was added after the popular basketball player endorsed the model. This \"Lugged\" version features a modified chunky sole with deep lugs.",3,4,2,(float)64.99),
("SH0015","Converse Women's Chuck Taylor All Star Lugged","One of the most stylish, timeless, and versatile sneakers ever created, the iconic Converse Chuck Taylor All Star. Originally released as the All Star in 1917, the Chuck Taylor name was added after the popular basketball player endorsed the model. This \"Lugged\" version features a modified chunky sole with deep lugs.",1,4,2,(float)64.99),
("SH0016","Vans x Supreme Sk8-Hi Pro (Fuck The World)","Inspired by the Vans Old Skool, the Vans Sk8-Hi is a classic high-top model with a timeless design. This legendary skateboarding sneaker has a durable tweed upper, Vans stripe, padded ankle collar, and Vans' signature waffle outsole.",2,6,1,(float)189.99),
("SH0017","Vans x Supreme Era Pro (Gaultier)","The Vans Era is very similar to the Vans Authentic, but it also boasts a padded collar. This staple for the Vans brand also features a combination of canvas & suede, vulcanized outsole and signature Waffle grip bottom. This limited edition is a collab model with Supreme and features a Gaultier design.",1,6,1,(float)219.99),
("SH0018","Vans Lampin Pro (Supreme)","Originally released in the 90s, the Vans Lampin is a skateboard sneaker with durable construction. they have a suede and corduroy upper, scaled sidewalls, metal eyelets, and signature rubber waffle outsoles.",2,6,1,(float)269.99),
("SH0019","Vans Sk8-Hi Reissue CAP (Florals)","Inspired by the Vans Old Skool, the Vans Sk8-Hi is a classic high-top model with a timeless design. This deconstructed cut-and-paste (CAP) version has mixed materials on the upper, zipper details, overlapping pieces, and a signature rubber waffle outsole.",1,6,2,(float)94.99),
("SH0020","Vans Classic Slip-On (I Heart)","The Vans Slip-On is practically the epitome of a casual shoe. They feature a low profile cut, paired with a timeless silhouette, and a Waffle sole. Plus, they also have a convenient on/off design.",3,6,2,(float)54.99),
("SH0021","Nike Air Max 270 React SE","The Nike Air Max 270 React combines some of Nike's latest cushioning innovations to create an ultra comfortable and supportive sneaker. They're constructed with a multi-layered no-sew textile upper, feature React cushioning, a 270 Max Air heel unit, rope laces, heel pull tab, and a rubber outsole.",1,1,1,(float)169.99),
("SH0022","Nike Waffle Racer","The Waffle Racer made its first debut in 1976 with a sleek design. The sporty, stylish design transfers easily from the track to the pavement. Plus, they are available in a wide array colors that all boast lightweight cushioning and support.",1,1,1,(float)84.99),
("SH0023","Nike Air Max 90 SP (Green Camo)","Originally named the Nike Air Max III, the Nike Air Max 90 is a popular runner known for its nearly incomparable comfort. Although you can currently find the Air Max 90 in an array of materials, the first model was designed with Duromesh, synthetic suede, and synthetic leather. Over twenty years after its debut, the Air Max 90 still retains its popularity with the \"Infrared\" model considered one of the greatest sneakers of all time.",1,1,1,(float)139.99),
("SH0024","Nike ACG Moc 3.0","Part of the Nike All Conditions Gear (ACG) collection, the Nike ACG Moc 3.0 is an shoe that has the characteristics of a moccasin infused with a Nike Air Footscape featuring premium upper materials.. They are built with a textile upper, feature sock-like quilted construction, EVA midsole technology, rubber toe for protection, and cross tread on the outsole for traction.",2,1,1,(float)89.99),
("SH0025","Nike Women's Blazer Mid '77","Released in 1972, the Nike Blazer was one of the first basketball sneakers Nike ever created. They were designed to compete with sneakers that were dominating the court, and feature a simple design with a large Nike Swoosh, and an outsole that's fused to the upper.",1,1,2,(float)99.99),
("SH0026","Nike Women's Air Max 270","The Nike Air Max 270 React combines some of Nike's latest cushioning innovations to create an ultra comfortable and supportive sneaker. They're constructed with a multi-layered no-sew textile upper, feature React cushioning, a 270 Max Air heel unit, rope laces, heel pull tab, and a rubber outsole.",3,1,2,(float)149.99),
("SH0027","Nike Women's Air Max Infinity","The Nike Air Max Infinity has a sleek modern style and flexible comfort. They have a upper comprised of synthetic suede and no-sew materials, Max Air unit for cushioning, pull tabs, and a rubber outsole.",3,1,2,(float)99.99),
("SH0028","Adidas Ozweego W","Inspired by Ozweego heritage, the latest version of the Adidas Ozweego fuses new and old design elements. They have a mesh upper with suede overlays, Adiprene cushioning, rope laces, and a rubber outsole.",1,2,2,(float)109.99),
("SH0029","Adidas Women's EdgeBounce W","The Adidas EdgeBounce has a Woman's essence in design elements. While keeping the integrity of a Woman's foot shape, the shoe features a more rounded heel for maximum comfort. A internal lift system built into the sock-liner for locked down stability, and a stretch mesh upper and spring-like cushioning for everlasting comfort.",1,2,2,(float)99.99)
            };

*/
using Newtonsoft.Json;
using shop_giay_server.models;
using System.Collections.Generic;
using System.Linq;

namespace shop_giay_server.data
{
    public class Seed
    {
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
        }
    }
}
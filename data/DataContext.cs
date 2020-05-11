using Microsoft.EntityFrameworkCore;
using shop_giay_server.models;

namespace shop_giay_server.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Shoes> Shoes { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using shop_giay_server.models;
using System;
using System.Linq;


namespace shop_giay_server.data
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shoes>()
                .HasOne<ShoesType>(s => s.ShoesType)
                .WithMany(t => t.ShoesList)
                .HasForeignKey(s => s.StyleId);

            modelBuilder.Entity<Shoes>()
                .HasOne<ShoesBrand>(s => s.ShoesBrand)
                .WithMany(t => t.ShoesList)
                .HasForeignKey(s => s.BrandId);
        }

        // Logging
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder =>
    {
        builder.AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
    });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Provider> Providers { get; set; }
        public DbSet<Shoes> Shoes { get; set; }
        public DbSet<ShoesType> ShoesTypes { get; set; }
        public DbSet<ShoesBrand> ShoesBrands { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<ShoesImage> ShoesImages { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<ImportDetail> ImportDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
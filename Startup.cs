using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shop_giay_server.data;
using shop_giay_server.models;
using shop_giay_server._Repository;
using AutoMapper;
using shop_giay_server.Dtos;
using shop_giay_server._Controllers;
using shop_giay_server.Handlers;
using shop_giay_server.data.Authentication;
using shop_giay_server.Crons;
using shop_giay_server.Controllers;

namespace shop_giay_server
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Generic Repository
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            // DB
            services.AddDbContext<DataContext>(x => x
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "https://ssneaker.azurewebsites.net/", "http://localhost:5001")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

            // add session to store info through middleware
            services.AddDistributedMemoryCache();
            services.AddSession(config =>
            {
                config.Cookie.Name = "shopgiay";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                config.IdleTimeout = new TimeSpan(0, 60, 0);
            });


            // add cron job services
            services.AddCronJob<DailyCron>(config =>
            {
                config.TimeZoneInfo = TimeZoneInfo.Local;
                config.CronExpression = @"0 0 * * *";  // cron start at 00:00 every day
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            // custom middleware
            app.UseMiddleware<SignInMiddleware>();          // check token if controller need authorize
            app.UseMiddleware<AuthenticationMiddleware>();  // check if user with request token has permission to complete action

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Size, SizeDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
            CreateMap<Color, ColorDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerReview, CustomerReviewDTO>().ReverseMap();
            CreateMap<CustomerType, CustomerTypeDTO>().ReverseMap();
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<Import, ImportDTO>().ReverseMap();
            CreateMap<ImportDetail, ImportDetailDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Stock, StockDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Provider, ProviderDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Sale, SaleDTO>().ReverseMap();
            CreateMap<SaleProduct, SaleProductDTO>().ReverseMap();
            CreateMap<Shoes, ShoesDTO>().ReverseMap();
            CreateMap<ShoesBrand, ShoesBrandDTO>().ReverseMap();
            CreateMap<ShoesImage, ShoesImageDTO>().ReverseMap();
            CreateMap<ShoesType, ShoesTypeDTO>().ReverseMap();
            CreateMap<Size, SizeDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Stock, StockDTO>().ReverseMap();

            CreateMap<Size, SizeLiteDTO>().ReverseMap();
            CreateMap<Address, AddressLiteDTO>().ReverseMap();
            CreateMap<Cart, CartLiteDTO>().ReverseMap();
            CreateMap<CartItem, CartItemLiteDTO>().ReverseMap();
            CreateMap<Color, ColorLiteDTO>().ReverseMap();
            CreateMap<Customer, CustomerLiteDTO>().ReverseMap();
            CreateMap<CustomerReview, CustomerReviewLiteDTO>().ReverseMap();
            CreateMap<CustomerType, CustomerTypeLiteDTO>().ReverseMap();
            CreateMap<Gender, GenderLiteDTO>().ReverseMap();
            CreateMap<Import, ImportLiteDTO>().ReverseMap();
            CreateMap<ImportDetail, ImportDetailLiteDTO>().ReverseMap();
            CreateMap<Order, OrderLiteDTO>().ReverseMap();
            CreateMap<Stock, StockLiteDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemLiteDTO>().ReverseMap();
            CreateMap<Payment, PaymentLiteDTO>().ReverseMap();
            CreateMap<Provider, ProviderLiteDTO>().ReverseMap();
            CreateMap<Role, RoleLiteDTO>().ReverseMap();
            CreateMap<Sale, SaleLiteDTO>().ReverseMap();
            CreateMap<Shoes, ShoesLiteDTO>().ReverseMap();
            CreateMap<ShoesBrand, ShoesBrandLiteDTO>().ReverseMap();
            CreateMap<ShoesImage, ShoesImageLiteDTO>().ReverseMap();
            CreateMap<ShoesType, ShoesTypeLiteDTO>().ReverseMap();
            CreateMap<Size, SizeLiteDTO>().ReverseMap();
            CreateMap<User, UserLiteDTO>().ReverseMap();
            CreateMap<Stock, StockLiteDTO>().ReverseMap();
            CreateMap<User, ResponseUserDto>().ReverseMap();


            // Shoes -> ResponseShoesDTO
            CreateMap<Shoes, ResponseShoesDTO>()
                .ForMember(des => des.name, opt => opt.MapFrom(s => s.Name))
                .ForMember(des => des.id, opt => opt.MapFrom(s => s.Id))
                .ForMember(des => des.description, opt => opt.MapFrom(s => s.Description))
                .ForMember(des => des.price, opt => opt.MapFrom(s => s.Price))
                .ForMember(des => des.isNew, opt => opt.MapFrom(s => s.IsNew))
                .ForMember(des => des.isOnSale, opt => opt.MapFrom(s => s.IsOnSale))
                .ForMember(
                    des => des.imagePath,
                    opt => opt.MapFrom((s, d) => s.ShoesImages.Count > 0 ? s.ShoesImages[0].ImagePath : "")
                )
                .ForMember(des => des.quantity, opt => opt.MapFrom((s, d) =>
                {
                    var total = 0;
                    foreach (var stock in s.Stocks)
                    {
                        total += stock.Instock;
                    }
                    return total;
                }))
                .ForMember(des => des.salePrice, opt => opt.MapFrom((s, d) =>
                {
                    return 0;
                }))
                .ForMember(des => des.description, opt => opt.MapFrom(s => s.ShoesType.Name));


            // Shoes -> ResponseShoesDetailDTO
            CreateMap<Shoes, ResponseShoesDetailDTO>()
                .ForMember(des => des.id, opt => opt.MapFrom(s => s.Id))
                .ForMember(des => des.code, opt => opt.MapFrom(s => s.Code))
                .ForMember(des => des.name, opt => opt.MapFrom(s => s.Name))
                .ForMember(des => des.description, opt => opt.MapFrom(s => s.Description))
                .ForMember(des => des.rating, opt => opt.MapFrom(s => s.Rating))
                .ForMember(des => des.ratingCount, opt => opt.MapFrom((s, d) =>
                {
                    // todo
                    return 0;
                }))
                .ForMember(des => des.styleName, opt => opt.MapFrom(s => s.ShoesType.Name))
                .ForMember(des => des.brandName, opt => opt.MapFrom(s => s.ShoesBrand.Name))
                .ForMember(des => des.genderName, opt => opt.MapFrom(s => s.Gender.Name))
                .ForMember(des => des.price, opt => opt.MapFrom(s => s.Price))
                .ForMember(des => des.isNew, opt => opt.MapFrom(s => s.IsNew))
                .ForMember(des => des.isOnSale, opt => opt.MapFrom(s => s.IsOnSale))
                .ForMember(des => des.images, opt => opt.MapFrom((s, d) =>
                {
                    var results = new List<String>();
                    foreach (var i in s.ShoesImages)
                    {
                        results.Add(i.ImagePath);
                    }
                    return results;
                }))
                .ForMember(des => des.sizes, opt => opt.MapFrom((s, d) =>
                {
                    var results = new List<String>();
                    foreach (var i in s.Stocks)
                    {
                        results.Add(i.Size.Name);
                    }
                    return results;
                }));


            CreateMap(typeof(Source<>), typeof(Destination<>)).ReverseMap();
        }
    }

}

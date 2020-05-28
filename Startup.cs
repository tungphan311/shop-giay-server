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
using shop_giay_server._Services;
using shop_giay_server.models;
using shop_giay_server._Repository;
using AutoMapper;
using shop_giay_server.Dtos;
using shop_giay_server._Controllers;

namespace shop_giay_server
{
    public class Startup
    {
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
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers();
            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();

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

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

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

            CreateMap<Shoes, ReponseShoesDTO>()
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
                .ForMember(des => des.quantity, opt => opt.MapFrom((s, d) => {
                    var total = 0;
                    foreach (var stock in s.Stocks)
                    {
                        total += stock.Instock;
                    }
                    return total;
                }))
                .ForMember(des => des.salePrice, opt => opt.MapFrom((s, d) => {
                    // todo
                    return 0;
                }))
                .ForMember(des => des.description, opt => opt.MapFrom(s => s.ShoesType.Name));


            CreateMap(typeof(Source<>), typeof(Destination<>)).ReverseMap();
        }
    }

}

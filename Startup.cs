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
            services.AddDbContext<DataContext>(x => x.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

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

            // app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Size, SizeDTO>();
            CreateMap<Address, AddressDTO>();
            CreateMap<Cart, CartDTO>();
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<Color, ColorDTO>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerReview, CustomerReviewDTO>();
            CreateMap<CustomerType, CustomerTypeDTO>();
            CreateMap<Gender, GenderDTO>();
            CreateMap<Import, ImportDTO>();
            CreateMap<ImportDetail, ImportDetailDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<Stock, StockDTO>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Payment, PaymentDTO>();

            CreateMap<Provider, ProviderDTO>();
            CreateMap<ProviderForCreateDTO, Provider>();

            CreateMap<Role, RoleDTO>();
            CreateMap<Sale, SaleDTO>();
            CreateMap<SaleProduct, SaleProductDTO>();
            CreateMap<Shoes, ShoesDTO>();
            CreateMap<ShoesBrand, ShoesBrandDTO>();
            CreateMap<ShoesImage, ShoesImageDTO>();
            CreateMap<ShoesType, ShoesTypeDTO>();
            CreateMap<Size, SizeDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Stock, StockDTO>();

            CreateMap(typeof(Source<>), typeof(Destination<>));
        }
    }

}

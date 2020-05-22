﻿using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    public class CustomController : GeneralController<Customer, CustomerDTO>
    {
        public CustomController(IAsyncRepository<Customer> repo, ILogger<CustomController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }
    }
}

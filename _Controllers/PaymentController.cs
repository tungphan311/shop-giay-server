﻿using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using shop_giay_server.data;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    public class PaymentController : GeneralController<Payment, PaymentDTO>
    {
        public PaymentController(IAsyncRepository<Payment> repo, ILogger<PaymentController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        { }
    }
}

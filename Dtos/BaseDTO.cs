using System;
using Microsoft.AspNetCore.Http;

namespace shop_giay_server.Dtos
{
    public class BaseDTO
    {
        public int Id { get; set; }

        public bool DeleteFlag { get; set; } = false;
    }
}

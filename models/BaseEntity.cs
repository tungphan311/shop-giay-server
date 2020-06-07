using System;
using Microsoft.AspNetCore.Http;

namespace shop_giay_server.models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool DeleteFlag { get; set; }
    }
}

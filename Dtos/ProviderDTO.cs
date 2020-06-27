using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ProviderDTO : ProviderLiteDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TIN { get; set; } // tax identification number
    }

    public class ProviderLiteDTO : BaseDTO
    {
        public List<ImportLiteDTO> Imports { get; set; }
    }
}
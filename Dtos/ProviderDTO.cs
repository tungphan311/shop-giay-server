using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ProviderDTO: ProviderLiteDTO
    {
        public string Name { get; set; }
    }

    public class ProviderLiteDTO : BaseDTO
    {
        public List<ImportLiteDTO> Imports { get; set; }
    }
}
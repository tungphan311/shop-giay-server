using System;
using System.Collections.Generic;


namespace shop_giay_server.Dtos
{
    public class ImportDTO: BaseDTO
    {
        public DateTime ImportDate { get; set; }

        public int TotalQuantity { get; set; }

        public float TotalCost { get; set; }

        public int ProviderId { get; set; }
        public ProviderDTO Provider { get; set; }

        public List<ImportDetailDTO> ImportDetails { get; set; }
    }
}
using System;
using System.Collections.Generic;


namespace shop_giay_server.Dtos
{
    public class ImportLiteDTO : BaseDTO
    {
        public DateTime ImportDate { get; set; }

        public int TotalQuantity { get; set; }

        public float TotalCost { get; set; }

        public int ProviderId { get; set; }
    }

    public class ImportDTO : ImportLiteDTO
    {
        public ProviderLiteDTO Provider { get; set; }

        public List<ImportDetailLiteDTO> ImportDetails { get; set; }
    }
}
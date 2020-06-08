using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class CustomerTypeLiteDTO: BaseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float Point { get; set; }
    }

    public class CustomerTypeDTO : CustomerTypeLiteDTO
    {
        public List<CustomerLiteDTO> Customers { get; set; }
    }
}
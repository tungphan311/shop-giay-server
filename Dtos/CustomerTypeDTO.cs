using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class CustomerTypeDTO: BaseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float Point { get; set; }

        public List<CustomerDTO> Customers { get; set; }
    }
}
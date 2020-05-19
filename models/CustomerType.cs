using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class CustomerType: BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float Point { get; set; }

        public List<Customer> Customers { get; set; }
    }
}
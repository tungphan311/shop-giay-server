using System.Linq;
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Address : LiteAddress
    {
        public Customer Customer { get; set; }

        public override string ToString()
        {
            var strs = new List<string>() { Street, Ward, District, City }
                .Where(c => string.IsNullOrEmpty(c));
            return string.Join(", ", strs);
        }
    }

    public class LiteAddress : BaseEntity
    {
        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public string Street { get; set; }

        public int CustomerId { get; set; }

        public string RecipientName { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public List<Order> Orders { get; set; }
    }
}
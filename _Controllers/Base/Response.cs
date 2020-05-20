using System;
using System.Collections.Generic;
using System.Linq;

namespace shop_giay_server._Controllers
{
    public class Response<ItemType>
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        // public List<ItemType> Data { get; set; }
        public string Data { get; set; }

        public Response(IEnumerable<ItemType> data, string code = "OK", string msg = "Success.")
        {
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(data.ToList()).ToString();
            Total = data.Count();
            Code = code;
            Msg = msg;
        }
    }
}

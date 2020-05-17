using System;
using System.Collections.Generic;
using System.Linq;

namespace shop_giay_server._Controllers
{
    public class Response<Item>
    {
        public string _code;
        public string _msg;
        public int _total;
        public IEnumerable<Item> _data;

        public Response(IEnumerable<Item> data, string code = "OK", string msg = "Success.")
        {
            _data = data;
            _total = data.Count();
            _code = code;
            _msg = msg;
        }
    }
}

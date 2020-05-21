using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace shop_giay_server._Controllers
{
    public class Response<ItemType>
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        // public List<ItemType> DataJson { get; set; }
        public string Data { get; set; }

        public Response(IEnumerable<ItemType> data, string code = "OK", string msg = "Success.")
        {
            var returnData = data.ToList();
            var convertSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 3,
            };

            // DataJson = returnData;
            Data = JsonConvert.SerializeObject(returnData, Formatting.None, convertSetting);
            Total = data.Count();
            Code = code;
            Msg = msg;
        }
    }
}

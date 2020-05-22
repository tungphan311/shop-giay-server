using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AutoMapper;
using shop_giay_server.models;
using shop_giay_server.Dtos;

namespace shop_giay_server._Controllers
{
    public class Response<ItemType>
    // where ItemType// : BaseEntity
    // where DTOType : BaseDTO
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        // public List<ItemType> DataJson { get; set; }
        public string Data { get; set; }

        public Response(IEnumerable<ItemType> data, string code = "OK", string msg = "Success.")
            : this(data.ToList(), code, msg)
        {

        }

        public Response(List<ItemType> data, string code = "OK", string msg = "Success.")
        {
            var convertSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 4,
            };

            // DataJson = data; 
            Data = JsonConvert.SerializeObject(data, Formatting.None, convertSetting);
            Total = data.Count();
            Code = code;
            Msg = msg;
        }

        public Response(object data, string code = "OK", string msg = "Success.")
        {
            var convertSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 4,
            };

            // DataJson = data; 
            Data = JsonConvert.SerializeObject(data, Formatting.None, convertSetting);
            Total = 1;
            Code = code;
            Msg = msg;
        }
    }
}

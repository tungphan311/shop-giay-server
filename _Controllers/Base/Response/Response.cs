using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AutoMapper;
using shop_giay_server.models;
using shop_giay_server.Dtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace shop_giay_server._Controllers
{
    interface IResponse<ItemType>
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        public string Data { get; set; }
    }

    public class Response<ItemType>: IResponse<ItemType>
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        public string Data { get; set; }


        public Response(IEnumerable<ItemType> data, string code = "OK", string msg = "Success.", int total = 0)
            : this(data.ToList(), code, msg, total)
        {

        }

        public Response(List<ItemType> data, string code = "OK", string msg = "Success.", int total = 0)
        {
            var convertSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 4,
            };

            Data = JsonConvert.SerializeObject(data, Formatting.None, convertSetting);
            Total = total == 0 ? data.Count() : total;
            Code = code;
            Msg = msg;
        }

        public Response(ItemType data, string code = "OK", string msg = "Success.")
        {
            var convertSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 4,
            };

            Data = JsonConvert.SerializeObject(data, Formatting.None, convertSetting);
            Total = 1;
            Code = code;
            Msg = msg;
        }


        // Convenience static methods
        public static Response<ItemType> Ok(IEnumerable<ItemType> data, int total)
        {
            return new Response<ItemType>(data, "OK", "Success.", total);
        }

        public static Response<ItemType> Ok(IEnumerable<ItemType> data)
        {
            return new Response<ItemType>(data, "OK", "Success.");
        }

        public static Response<ItemType> Ok(ItemType data)
        {
            return new Response<ItemType>(data, "OK", "Success.");
        }

        public static Response<ItemType> OkDeleted(ItemType data, string msg = "Deleted.")
        {
            return new Response<ItemType>(data, "OK", msg);
        }

        public static Response<ItemType> BadRequest(string msg = "Invalid request.")
        {
            var empty = new List<ItemType>();
            return new Response<ItemType>(empty, "ERROR", msg);
        }

        public static Response<ItemType> NotFound()
        {
            var empty = new List<ItemType>();
            return new Response<ItemType>(empty, "ERROR", "Not Found.");
        }

        internal static object Ok(EntityEntry<Stock> stockResult)
        {
            throw new NotImplementedException();
        }
    }

    
}

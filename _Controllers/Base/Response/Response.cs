﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AutoMapper;
using shop_giay_server.models;
using shop_giay_server.Dtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace shop_giay_server._Controllers
{
    interface IResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        public string Data { get; set; }
    }

    public class ResponseDTO: IResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        public string Data { get; set; }


        public ResponseDTO(IEnumerable<object> data, string code = "OK", string msg = "Success.", int total = 0)
            : this(data.ToList(), code, msg, total)
        {

        }

        public ResponseDTO(List<object> data, string code = "OK", string msg = "Success.", int total = 0)
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

        public ResponseDTO(object data, string code = "OK", string msg = "Success.")
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
        public static ResponseDTO Ok(IEnumerable<object> data, int total)
        {
            return new ResponseDTO(data, "OK", "Success.", total);
        }

        public static ResponseDTO Ok(IEnumerable<object> data)
        {
            return new ResponseDTO(data, "OK", "Success.");
        }

        public static ResponseDTO Ok(object data)
        {
            return new ResponseDTO(data, "OK", "Success.");
        }

        public static ResponseDTO OkDeleted(object data, string msg = "Deleted.")
        {
            return new ResponseDTO(data, "OK", msg);
        }

        public static ResponseDTO BadRequest(string msg = "Invalid request.", string data = "")
        {
            return new ResponseDTO(data, "ERROR", msg);
        }

        public static ResponseDTO NotFound()
        {
            var empty = new List<object>();
            return new ResponseDTO(empty, "ERROR", "Not Found.");
        }

        internal static object Ok(EntityEntry<Stock> stockResult)
        {
            throw new NotImplementedException();
        }
    }

    
}

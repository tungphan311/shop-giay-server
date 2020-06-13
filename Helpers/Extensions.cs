using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Primitives;

namespace shop_giay_server.Helpers
{

    public static class IQueryCollectionModifyExtension 
    {

        public static IQueryCollection AddQueryItem(
            this IQueryCollection queryCollection, 
            KeyValuePair<string, StringValues> item) 
            {
                try 
                {
                    var query = (QueryCollection)queryCollection;
                    Dictionary<string, StringValues> dict = query.ToDictionary(c => c.Key, c => c.Value);

                    if (dict.ContainsKey(item.Key)) 
                    {
                        var dictValue = dict[item.Key];
                        foreach (var str in item.Value) 
                        {
                            if (!dictValue.Contains(str)) 
                            {
                                dictValue.Append(str);
                            }
                        }
                        dict[item.Key] = dictValue;
                    }
                    else 
                    {
                        dict.Add(item.Key, item.Value);
                    }
                    
                    IQueryCollection result = new QueryCollection(dict);
                    return result;
                }
                catch 
                {
                    Console.WriteLine("ERROR: AddQueryItem return null.");
                    return queryCollection;
                }
            }

    }


}
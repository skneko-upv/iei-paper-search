using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IEIPaperSearch.Pages
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Objects,
            });
        }

        public static T? Get<T>(this ITempDataDictionary tempData, string key)
            where T : class
        {
            object? o;
            tempData.TryGetValue(key, out o);
            return o is null ? default : JsonConvert.DeserializeObject<T>((string)o, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Objects,
            });
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeleniumAutotest
{
    public static class SystemExtension
    {
        public static T Clone<T>(this T source)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var serialized = JsonConvert.SerializeObject(source, settings);
            return JsonConvert.DeserializeObject<T>(serialized, settings);
        }
    }
}

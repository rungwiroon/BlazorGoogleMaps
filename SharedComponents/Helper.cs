using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    public static class Helper
    {
        internal static Task<TRes> MyInvokeAsync<TRes>(string identifier, params object[] args)
        {
            var argsJson = JsonConvert.SerializeObject(args,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            });

            return JSRuntime.Current.InvokeAsync<TRes>(identifier, argsJson);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapsComponents
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection service)
        {
            service.AddSingleton<MapEventJsInterop>();

            return service;
        }
    }
}

using GoogleMapsComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServerSideDemo;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);

        // Adds the service to use bootstrap loader for Google API JS. 
        services.AddBlazorGoogleMaps(new EnvVariableBlazorGoogleMapsKeyService());
        //services.AddBlazorGoogleMaps(new MapApiLoadOptions("GOOGLE_MAPS_API_KEY")
        //{
        //    Version = "beta",
        //    MapId = "MAP_ID"
        //});
        // Or manually set version and libraries for entire app:
        //services.AddBlazorGoogleMaps(new GoogleMapsComponents.Maps.MapApiLoadOptions("GOOGLE_MAPS_API_KEY")
        //{
        //    Version = "beta",
        //    Libraries = "places,visualization,drawing,marker",
        //    MapId = "MAP_ID"
        //});
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}

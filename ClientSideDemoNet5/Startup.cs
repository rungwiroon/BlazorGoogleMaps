using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ClientSideDemoNet5
{
    public class Startup {

        private static async Task Main(string[] args) {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();

        }

    }
}

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Serilog;
using Serilog.Debugging;

namespace CV2WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            SelfLog.Enable(m => Console.Error.WriteLine(m));

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.BrowserConsole()
                .CreateLogger();

            Log.Information("Hello, browser!");
            Log.Warning("Received strange response {@Response} from server", new { Username = "example", Cats = 7 });

            builder.Services.AddMudServices();
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            await builder.Build().RunAsync();

            Log.Information("Hello, browser!");
        }
    }
}

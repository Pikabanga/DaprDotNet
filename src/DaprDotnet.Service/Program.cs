using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace DaprDotnet.Service;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    // Setup a HTTP/2 endpoint without TLS.
                    options.ListenLocalhost(5050, o => o.Protocols =
                    HttpProtocols.Http2);
                });

                webBuilder.UseStartup<Startup>();
            });
}

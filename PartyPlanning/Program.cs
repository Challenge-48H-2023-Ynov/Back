using Microsoft.AspNetCore;

namespace PartyPlanning;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IWebHostBuilder CreateHostBuilder(string[] args) =>

        WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>();
}
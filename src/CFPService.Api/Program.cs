using CFPService.Infrastructure.Extensions;

namespace CFPService.Api;

public class Program
{
    public static void Main()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());

        builder.Build().MigrateUp().Run();
    }
}
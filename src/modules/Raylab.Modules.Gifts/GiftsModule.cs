using Microsoft.Extensions.DependencyInjection;
using Kantaiko.Hosting.Modularity;
using Raylab.Modules.Gifts.Providers;
using Raylab.Modules.Gifts.Providers.WaifuPics;
using Raylab.Modules.Gifts.Services;
using Replikit.Core.Modules;
using Replikit.Extensions.Views;

namespace Raylab.Modules.Gifts;

public class GiftsModule : ReplikitModule
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<ViewsModule>();
        services.AddSingleton<HttpClient>();
        
        services.AddScoped<IGiftProvider, WaifuPicsGiftProvider>();
        services.AddScoped<IGiftService, GiftService>();
    }
}

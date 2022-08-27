using Kantaiko.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Raylab.Modules.Gifts.Providers;
using Raylab.Modules.Gifts.Providers.WaifuPics;
using Raylab.Modules.Gifts.Services;
using Replikit.Core.Modularity;
using Replikit.Extensions.Views;

namespace Raylab.Modules.Gifts;

public class GiftsModule : ReplikitModule
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<ReplikitViewsModule>();
        services.AddSingleton<HttpClient>();
        
        services.AddScoped<IGiftProvider, WaifuPicsGiftProvider>();
        services.AddScoped<IGiftService, GiftService>();
    }
}

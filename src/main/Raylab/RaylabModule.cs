using Kantaiko.Hosting.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Raylab.Modules.Gifts;
using Replikit.Abstractions.Adapters.Loader;
using Replikit.Adapters.Telegram;
using Replikit.Core.Modules;

namespace Raylab;

public class RaylabModule : ReplikitModule
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<GiftsModule>();
    }

    protected override void ConfigureAdapters(IAdapterLoaderOptions options)
    {
        options.AddTelegram();
    }
}

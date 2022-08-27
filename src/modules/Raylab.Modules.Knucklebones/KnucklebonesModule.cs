using Kantaiko.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Replikit.Core.Modularity;
using Replikit.Extensions.Views;

namespace Raylab.Modules.Knucklebones;

public class KnucklebonesModule : ReplikitModule
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<ReplikitViewsModule>();
    }
}

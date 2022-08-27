using Kantaiko.Modularity;
using Microsoft.Extensions.Hosting;
using Replikit.Adapters.Telegram;
using Replikit.Core.Hosting;
using Raylab.Modules.Gifts;
using Raylab.Modules.Knucklebones;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureReplikit(replikit =>
{
    replikit.Services.AddModule<GiftsModule>();
    replikit.Services.AddModule<KnucklebonesModule>();

    replikit.ConfigureAdapters(adapters => adapters.AddTelegram());
});

builder.ConfigureDevelopmentUserSecrets<Program>();

var host = builder.Build();

host.Run();

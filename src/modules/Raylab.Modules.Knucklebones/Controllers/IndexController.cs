using Raylab.Modules.Knucklebones.Presentation;
using Replikit.Core.Controllers;
using Replikit.Core.Controllers.Patterns;
using Replikit.Extensions.Views;

namespace Raylab.Modules.Knucklebones.Controllers;

internal class IndexController : Controller
{
    private readonly IViewManager _viewManager;

    public IndexController(IViewManager viewManager)
    {
        _viewManager = viewManager;
    }

    [Command("knucklebones", "dice-game")]
    public async Task CreateLobby()
    {
        await _viewManager.SendViewAsync<KnucklebonesView>(Channel.Id, 
            x => x.SetLobbyOwner(Account), Context.CancellationToken);
    }
}
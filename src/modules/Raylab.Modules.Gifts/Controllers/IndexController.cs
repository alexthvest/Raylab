using Raylab.Modules.Gifts.Presentation;
using Replikit.Core.Controllers;
using Replikit.Core.Controllers.Patterns;
using Replikit.Extensions.Views;

namespace Raylab.Modules.Gifts.Controllers;

internal class IndexController : Controller
{
    private readonly IViewManager _viewManager;

    public IndexController(IViewManager viewManager)
    {
        _viewManager = viewManager;
    }
    
    [Command("gift")]
    public async Task SendGift()
    {
        await _viewManager.SendViewAsync<GiftView>(Channel.Id, CancellationToken);
    }
}

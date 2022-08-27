using Raylab.Modules.Gifts.Services;
using Replikit.Abstractions.Messages.Models.TextTokens;
using Replikit.Extensions.State;
using Replikit.Extensions.Views;

namespace Raylab.Modules.Gifts.Presentation;

internal class GiftView : View
{
    private readonly IState<GiftViewState> _state;
    private readonly IGiftService _giftService;

    public GiftView(IState<GiftViewState> state, IGiftService giftService)
    {
        _state = state;
        _giftService = giftService;
    }

    public void ChangeGiftState(GiftState state)
    {
        _state.Value.State = state;
    }

    public override async Task<ViewMessage> RenderAsync(CancellationToken cancellationToken)
    {
        var view = _state.Value.State switch
        {
            GiftState.Opened => await RenderOpened(cancellationToken),
            GiftState.Returned => RenderReturned(),
            _ => RenderClosed()
        };

        return view;
    }

    private async Task<ViewMessage> RenderOpened(CancellationToken cancellationToken)
    {
        try
        {
            var url = await _giftService.GetRandomUrlAsync(cancellationToken);
            var link = TextToken.Link("\u00AD", new Uri(url));

            var view = new ViewMessage
            {
                Text = link,
                Actions =
                {
                    Action("Вернуть обратно", _ => ChangeGiftState(GiftState.Returned))
                }
            };

            return view;
        }
        catch
        {
            return "Не удалось открыть подарок";
        }
    }

    private ViewMessage RenderReturned() => "Вы вернули подарок";

    private ViewMessage RenderClosed() => new()
    {
        Text = "Вам прислали подарок",
        Actions =
        {
            Action("Открыть", _ => ChangeGiftState(GiftState.Opened))
        }
    };
}

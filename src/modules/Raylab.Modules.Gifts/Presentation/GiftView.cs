using Raylab.Modules.Gifts.Services;
using Replikit.Abstractions.Messages.Builder;
using Replikit.Abstractions.Messages.Models.Tokens;
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

    [Action]
    public void ChangeGiftState(GiftState state)
    {
        _state.Value.State = state;
    }

    public override async Task<ViewResult> RenderAsync(CancellationToken cancellationToken)
    {
        var view = _state.Value.State switch
        {
            GiftState.Opened => await RenderOpened(cancellationToken),
            GiftState.Returned => RenderReturned(),
            _ => RenderClosed()
        };

        return view;
    }

    private async Task<ViewResult> RenderOpened(CancellationToken cancellationToken)
    {
        try
        {
            var url = await _giftService.GetRandomUrlAsync(cancellationToken);
            var link = new LinkTextToken("\u00AD", url);

            var view = CreateBuilder()
                .WithText(link).AddTextLine()
                .AddAction("Вернуть обратно", () => ChangeGiftState(GiftState.Returned));

            return view;
        }
        catch
        {
            return CreateBuilder()
                .AddTextLine("Не удалось открыть подарок");
        }
    }

    private ViewResult RenderReturned()
    {
        return CreateBuilder().AddTextLine("Вы вернули подарок");
    }

    private ViewResult RenderClosed()
    {
        var view = CreateBuilder()
            .AddTextLine("Вам прислали подарок!")
            .AddAction("Открыть", () => ChangeGiftState(GiftState.Opened));

        return view;
    }
}

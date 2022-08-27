using Raylab.Modules.Knucklebones.Mechanics;
using Replikit.Abstractions.Accounts.Models;
using Replikit.Abstractions.Messages.Models.TextTokens;
using Replikit.Extensions.State;
using Replikit.Extensions.Views;
using Replikit.Extensions.Views.Actions;

namespace Raylab.Modules.Knucklebones.Presentation;

internal class KnucklebonesView : View
{
    private readonly IState<KnucklebonesState> _state;

    public KnucklebonesView(IState<KnucklebonesState> state)
    {
        _state = state;
    }

    public void SetLobbyOwner(AccountInfo account)
    {
        _state.Value.LobbyState.Owner = new KnucklebonesPlayer(account);
    }

    public void JoinLobby(IViewActionContext context)
    {
        context.SuppressAutoUpdate();
        
        if (_state.Value.LobbyState.Opponent is null &&
            _state.Value.LobbyState.Owner.Account.Id != context.Event.Account.Id)
        {
            _state.Value.LobbyState.Opponent = new KnucklebonesPlayer(context.Event.Account);
            context.Update();
        }
    }

    public void LeaveLobby(IViewActionContext context)
    {
        context.SuppressAutoUpdate();

        if (_state.Value.LobbyState.Opponent?.Account.Id == context.Event.Account.Id)
        {
            _state.Value.LobbyState.Opponent = null;
            context.Update();
        }
    }

    public void StartGame(IViewActionContext context)
    {
        context.SuppressAutoUpdate();
        
        if (_state.Value.LobbyState.Opponent is not null &&
            _state.Value.LobbyState.Owner.Account.Id == context.Event.Account.Id)
        {
            _state.Value.SessionState = GameSessionState.Started;
            _state.Value.GameState = new KnucklebonesGameState(_state.Value.LobbyState.Owner, _state.Value.LobbyState.Opponent);
            
            context.Update();
        }
    }

    public void ThrowDice(IViewActionContext context)
    {
        context.SuppressAutoUpdate();
        
        if (_state.Value.GameState.Turn.Current.Account.Id != context.Event.Account.Id)
        {
            return;
        }

        var dice = Random.Shared.Next(1, 7);

        _state.Value.GameState.TurnState = GameTurnState.WaitingPlace;
        _state.Value.GameState.UpdateDice(dice);

        context.Update();
    }

    public void SelectDicePlace(IViewActionContext context, int column)
    {
        context.SuppressAutoUpdate();
        
        if (_state.Value.GameState.Turn.Current.Account.Id != context.Event.Account.Id)
        {
            return;
        }

        if (_state.Value.GameState.Turn.Current.Field.AddDice(column, _state.Value.GameState.Dice))
        {
            var opponent = _state.Value.GameState.Turn.Next.Current;
            opponent.Field.RemoveDice(column, _state.Value.GameState.Dice);

            if (_state.Value.GameState.Turn.Current.Field.Filled)
            {
                _state.Value.SessionState = GameSessionState.Ended;
                context.Update();
                return;
            }

            _state.Value.GameState.TurnState = GameTurnState.WaitingDice;
            _state.Value.GameState.NextTurn();

            context.Update();
        }
    }

    public override ViewMessage Render()
    {
        return _state.Value.SessionState switch
        {
            GameSessionState.Started => RenderGame(),
            GameSessionState.Ended => RenderEnd(),
            _ => RenderLobby()
        };
    }

    private ViewMessage RenderLobby()
    {
        var owner = _state.Value.LobbyState.Owner;
        var opponent = _state.Value.LobbyState.Opponent;

        var view = new ViewMessage
        {
            Text =
            {
                TextToken.Bold(MessageFormatHelpers.FormatAccount(owner.Account)),
                " VS ",
                TextToken.Bold(opponent is not null ? MessageFormatHelpers.FormatAccount(opponent.Account) : "???"),
            },
            Actions =
            {
                {
                    Action("Присоединиться", context => JoinLobby(context)),
                    Action("Покинуть", context => LeaveLobby(context))
                },
                {
                    Action("Начать", context => StartGame(context))
                }
            }
        };

        return view;
    }

    private ViewMessage RenderGame()
    {
        var view = new ViewMessage();

        for (var row = 3 - 1; row >= 0; row--)
        {
            for (var column = 0; column < 3; column++)
            {
                var dice = _state.Value.LobbyState.Owner.Field.GetDice(column, row);
                view.Text.Add(MessageFormatHelpers.FormatDice(dice));
            }

            if (row == 1)
            {
                var owner = _state.Value.LobbyState.Owner;
                view.Text.Add(TextToken.Bold($"     {MessageFormatHelpers.FormatAccount(owner.Account)} ({owner.Score})"));
            }

            view.Text.Add(TextToken.Line());
        }

        view.Text.Add(TextToken.Line());

        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 3; column++)
            {
                var dice = _state.Value.LobbyState.Opponent!.Field.GetDice(column, row);
                view.Text.Add(MessageFormatHelpers.FormatDice(dice));
            }

            if (row == 1)
            {
                var opponent = _state.Value.LobbyState.Opponent!;
                view.Text.Add(TextToken.Bold($"     {MessageFormatHelpers.FormatAccount(opponent.Account)} ({opponent.Score})"));
            }

            view.Text.Add(TextToken.Line());
        }

        view.Text.AddRange(new[]
        {
            TextToken.Line(),
            TextToken.Bold("▫Сейчас ходит: "), 
            TextToken.Line(MessageFormatHelpers.FormatAccount(_state.Value.GameState.Turn.Current.Account)),
            TextToken.Bold("▫Значение костей: "), 
            TextToken.Line(MessageFormatHelpers.FormatDice(_state.Value.GameState.Dice)), 
            TextToken.Bold("▫Состояние хода: "), 
            TextToken.Line(_state.Value.GameState.TurnState == GameTurnState.WaitingDice ? "Бросание костей" : "Выбор столбца")
        });

        if (_state.Value.GameState.TurnState == GameTurnState.WaitingDice)
        {
            view.Actions.Add(Action("Бросить кости", context => ThrowDice(context)));
        }
        else if (_state.Value.GameState.TurnState == GameTurnState.WaitingPlace)
        {
            view.Actions.Add(
                Action("1", context => SelectDicePlace(context, 0)),
                Action("2", context => SelectDicePlace(context, 1)),
                Action("3", context => SelectDicePlace(context, 2))
            );
        }

        return view;
    }

    private ViewMessage RenderEnd()
    {
        var winner = _state.Value.GameState.Turn.Current;
        var looser = _state.Value.GameState.Turn.Next.Current;

        return $"{MessageFormatHelpers.FormatAccount(winner.Account)} победил {winner.Score} - {looser.Score}";
    }
}

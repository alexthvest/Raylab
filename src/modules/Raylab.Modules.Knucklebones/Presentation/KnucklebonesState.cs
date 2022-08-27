using Raylab.Modules.Knucklebones.Mechanics;

namespace Raylab.Modules.Knucklebones.Presentation;

internal class KnucklebonesLobbyState
{
    public KnucklebonesPlayer Owner { get; set; } = default!;
    
    public KnucklebonesPlayer? Opponent { get; set; }
}

internal class KnucklebonesGameState
{
    public KnucklebonesGameState(KnucklebonesPlayer owner, KnucklebonesPlayer opponent)
    {
        Turn = new ChainTurn<KnucklebonesPlayer>(owner, opponent);
    }
    
    public GameTurnState TurnState { get; set; } = GameTurnState.WaitingDice;
    
    public ChainTurn<KnucklebonesPlayer> Turn { get; private set; }
    
    public int Dice { get; private set; }

    public void NextTurn()
    {
        Turn = Turn.Next;
    }
    
    public void UpdateDice(int value)
    {
        if (value is > 0 and < 7)
        {
            Dice = value;
        }
    }
}

internal class KnucklebonesState
{
    public GameSessionState SessionState { get; set; } = GameSessionState.Lobby;

    public KnucklebonesLobbyState LobbyState { get; } = new();
    
    public KnucklebonesGameState GameState { get; set; }
}
using Replikit.Abstractions.Accounts.Models;

namespace Raylab.Modules.Knucklebones.Mechanics;

internal record KnucklebonesPlayer(AccountInfo Account)
{
    public KnucklebonesPlayerField Field { get; } = new();

    public int Score => Field.Score;
}

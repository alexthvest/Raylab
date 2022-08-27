using Replikit.Abstractions.Accounts.Models;

namespace Raylab.Modules.Knucklebones.Presentation;

internal static class MessageFormatHelpers
{
    public static string FormatAccount(AccountInfo accountInfo)
    {
        return accountInfo.Username ?? accountInfo.FirstName ?? $"#{accountInfo.Id}";
    }

    public static string FormatDice(int value)
    {
        var emojies = new[] { "0️⃣", "1️⃣", "2️⃣", "3️⃣", "4️⃣", "5️⃣", "6️⃣" };
        return emojies[value];
    }
}

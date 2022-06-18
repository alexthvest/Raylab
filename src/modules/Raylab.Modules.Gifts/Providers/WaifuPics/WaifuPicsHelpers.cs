namespace Raylab.Modules.Gifts.Providers.WaifuPics;

internal static class WaifuPicsHelpers
{
    private static readonly Dictionary<string, string[]> Categories = new()
    {
        ["sfw"] = new[]
        {
            "waifu", 
            "neko", 
            "hug",
            "awoo",
            "kiss",
            "lick",
            "blush",
            "smile",
            "happy",
            "dance"
        },
        ["nsfw"] = new[]
        {
            "waifu", 
            "neko", 
            "blowjob"
        }
    };

    public static string GetRandomUrl()
    {
        var typeChance = Random.Shared.Next(0, 100);
        var type = typeChance < 10 ? "nsfw" : "sfw";

        var categories = Categories[type];
        var categoryIndex = Random.Shared.Next(0, categories.Length);
        var category = categories[categoryIndex];
        
        return GetBaseUrl(type, category);
    }

    public static string GetBaseUrl(string type, string category)
    {
        return $"https://api.waifu.pics/{type}/{category}";
    }
}

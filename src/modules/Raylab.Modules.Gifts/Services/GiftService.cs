using Raylab.Modules.Gifts.Providers;

namespace Raylab.Modules.Gifts.Services;

internal class GiftService : IGiftService
{
    private readonly IReadOnlyList<IGiftProvider> _giftProviders;

    public GiftService(IEnumerable<IGiftProvider> giftProviders)
    {
        _giftProviders = giftProviders.ToList();
    }
    
    public async Task<string> GetRandomUrlAsync(CancellationToken cancellationToken = default)
    {
        var index = Random.Shared.Next(0, _giftProviders.Count);
        var provider = _giftProviders[index];

        return await provider.GetUrlAsync(cancellationToken);
    }
}

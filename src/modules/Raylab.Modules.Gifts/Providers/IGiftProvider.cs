namespace Raylab.Modules.Gifts.Providers;

public interface IGiftProvider
{
    Task<string> GetUrlAsync(CancellationToken cancellationToken = default);
}

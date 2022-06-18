namespace Raylab.Modules.Gifts.Services;

public interface IGiftService
{
    Task<string> GetRandomUrlAsync(CancellationToken cancellationToken = default);
}

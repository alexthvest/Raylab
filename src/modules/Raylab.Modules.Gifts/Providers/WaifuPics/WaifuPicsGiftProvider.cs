using System.Net.Http.Json;

namespace Raylab.Modules.Gifts.Providers.WaifuPics;

internal class WaifuPicsGiftProvider : IGiftProvider
{
    private readonly HttpClient _httpClient;

    public WaifuPicsGiftProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetUrlAsync(CancellationToken cancellationToken = default)
    {
        for (var retries = 0; retries < 5; retries++)
        {
            var url = WaifuPicsHelpers.GetRandomUrl();
            var response = await _httpClient.GetFromJsonAsync<WaifuPicsResponse>(url, cancellationToken);

            if (response?.Url is not null)
            {
                return response.Url;
            }
        }

        throw new Exception("Unable to get image");
    }
}
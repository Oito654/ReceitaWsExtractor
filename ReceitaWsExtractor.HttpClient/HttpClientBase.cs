using Microsoft.Extensions.Options;
using ReceitaWsExtractor.HttpClient.Configuration;

namespace ReceitaWsExtractor.HttpClient;

public class HttpClientBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppSettingOptions _options;

    protected HttpClientBase()
    {
        throw new ArgumentNullException(nameof(_httpClientFactory));
        throw new ArgumentNullException(nameof(_options));
    }

    public HttpClientBase(
        IHttpClientFactory httpClientFactory, IOptions<AppSettingOptions> options)
    {
        _httpClientFactory = httpClientFactory
            ?? throw new ArgumentNullException(nameof(httpClientFactory));

        _options = (AppSettingOptions)(options
            ?? throw new ArgumentNullException(nameof(options)));
    }

    public System.Net.Http.HttpClient GetHttpClient()
    {
        var client = _httpClientFactory.CreateClient();

        client.Timeout = TimeSpan.FromSeconds(100);

        return client;
    }
}

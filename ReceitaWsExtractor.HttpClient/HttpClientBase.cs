using Microsoft.Extensions.Options;
using ReceitaWsExtractor.HttpClient.Configuration;

namespace ReceitaWsExtractor.HttpClient;

public class HttpClientBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientBase(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory
            ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    protected HttpClientBase()
    {
        throw new ArgumentNullException(nameof(_httpClientFactory));
    }

    public System.Net.Http.HttpClient GetHttpClient()
    {
        var client = _httpClientFactory.CreateClient();

        client.Timeout = TimeSpan.FromSeconds(100);

        return client;
    }
}

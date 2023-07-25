using Newtonsoft.Json;
using ReceitaWsExtractor.Application.HttpClient;
using ReceitaWsExtractor.HttpClient.Configuration;
using System.Dynamic;
using System.Net.Http;

namespace ReceitaWsExtractor.HttpClient;

public class ConsultaCnpjHttpClient : HttpClientBase, IConsultaCnpjHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppSettingOptions _options;

    private ConsultaCnpjHttpClient() : base()
    {
        throw new ArgumentNullException(nameof(_httpClientFactory));
        throw new ArgumentNullException(nameof(_options));
    }

    public ConsultaCnpjHttpClient(IHttpClientFactory httpClientFactory, AppSettingOptions options)
    {
        _httpClientFactory = httpClientFactory
           ?? throw new ArgumentNullException(nameof(httpClientFactory));

        _options = options
            ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<string> GetCnpjDataAsync(string cnpj)
    {
        dynamic details = new ExpandoObject();
        details.route = $"{_options.BaseUrl}/cnpj/{cnpj}";

        try
        {
            var client = GetHttpClient();

            var response = await client.GetAsync(cnpj);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = content.ToString();

                return result;
            }
            throw new Exception(content.ToString());
        }
        catch(Exception ex) 
        {
            throw new Exception(cnpj, ex);
        }
    }
}

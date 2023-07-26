using Newtonsoft.Json;
using ReceitaWsExtractor.Application.HttpClient;
using ReceitaWsExtractor.HttpClient.Configuration;
using System.Dynamic;
using System.Net.Http;

namespace ReceitaWsExtractor.HttpClient;

public class ConsultaCnpjHttpClient : HttpClientBase, IConsultaCnpjHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ConsultaCnpjHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory
           ?? throw new ArgumentNullException(nameof(httpClientFactory));

    }

    private ConsultaCnpjHttpClient() : base()
    {
        throw new ArgumentNullException(nameof(_httpClientFactory));
    }

    public async Task<string> GetCnpjDataAsync(string cnpj)
    {
        dynamic details = new ExpandoObject();

        //Normalmente a BaseUrl seria armazenada dentro de uma variavél de ambiente ou do AppSetting.
        details.route = $"https://receitaws.com.br/v1/cnpj/{cnpj}";

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

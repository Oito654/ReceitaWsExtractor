namespace ReceitaWsExtractor.Application.HttpClient;

public interface IConsultaCnpjHttpClient
{
    Task<string> GetCnpjDataAsync(string cnpj);
}

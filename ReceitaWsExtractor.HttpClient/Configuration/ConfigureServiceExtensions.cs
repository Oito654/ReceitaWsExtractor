using Microsoft.Extensions.DependencyInjection;
using ReceitaWsExtractor.Application.HttpClient;

namespace ReceitaWsExtractor.HttpClient.Configuration;

public static class ConfigureServiceExtensions
{
    public static IServiceCollection AddHttpClientReceitaWs(
        this IServiceCollection services)
    {
        services.AddScoped<IConsultaCnpjHttpClient, ConsultaCnpjHttpClient>();

        services.AddMemoryCache();

        return services;
    }
}

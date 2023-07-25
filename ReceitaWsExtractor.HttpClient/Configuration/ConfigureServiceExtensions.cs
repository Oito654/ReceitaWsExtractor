using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReceitaWsExtractor.Application.HttpClient;

namespace ReceitaWsExtractor.HttpClient.Configuration;

public static class ConfigureServiceExtensions
{
    public static IServiceCollection AddHttpClient(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IConsultaCnpjHttpClient, ConsultaCnpjHttpClient>();

        services.AddMemoryCache();

        return services;
    }
}

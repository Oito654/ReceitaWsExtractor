using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReceitaWsExtractor.Domain.Interfaces;
using ReceitaWsExtractor.Infra.Context;
using ReceitaWsExtractor.Infra.Repositories;

namespace ReceitaWsExtractor.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReceitaWsExtractorDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ReceitaWsExtractorDbContext).Assembly.FullName)
                )
        );

        services.AddScoped<IClientRepository, ClientRepository>();

        return services;
    }
}

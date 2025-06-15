using Microsoft.Extensions.DependencyInjection;
using LindebergsHealth.Domain.Termine;
using LindebergsHealth.Infrastructure.Repositories;

namespace LindebergsHealth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITermineRepository, TermineRepository>();
            // Hier ggf. weitere Repositories und Infrastruktur-Services registrieren
            return services;
        }
    }
}

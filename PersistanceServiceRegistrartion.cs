using ECommerce.Connection;
using ECommerce.Contracts;

namespace ECommerce
{
    public static class PersistanceServiceRegistrartion
    {
        public static IServiceCollection AddPatientPersistenceServices(this IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}

using bom.Managers.ItemMasterSales.Abstractions;
using bom.Managers.ItemMasterSales.Implementations;
using bom.Repositories.ItemMasterSales.Abstractions;
using bom.Repositories.ItemMasterSales.Implementations;

namespace bom.API.AppSetup
{
    public static class AppSetup
    {
        public static void RegisterDependency(IServiceCollection services, string connectionString)
        {
            RegisterManagers(services);
            RegisterRepository(services, connectionString);
        }

        private static void RegisterManagers(IServiceCollection services)
        {
            services.AddScoped<IItemMasterSalesManager, ItemMasterSalesManager>();
            
        }

        private static void RegisterRepository(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IItemMasterSalesRepository>(x => new ItemMasterSalesRepository(connectionString));
            
        }
    }
}

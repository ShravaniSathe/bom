using bom.Managers.ItemMasterSales.Abstractions;
using bom.Managers.ItemMasterSales.Implementations;
using bom.Repositories.ItemMasterSales.Abstractions;
using bom.Repositories.ItemMasterSales.Implementations;
using bom.Managers.ItemMasterRawMaterials.Abstractions;
using bom.Managers.ItemMasterRawMaterials.Implementations;
using bom.Repositories.ItemMasterRawMaterials.Abstractions;
using bom.Repositories.ItemMasterRawMaterials.Implementations;
using bom.Managers.SubAssemblies.Abstractions;
using bom.Managers.SubAssemblies.Implementations;
using bom.Repositories.SubAssemblies.Abstractions;
using bom.Repositories.SubAssemblies.Implementations;
using bom.Managers.BOMStructures.Abstractions;
using bom.Managers.BOMStructures.Implementations;
using bom.Repositories.BOMStructures.Abstractions;
using bom.Repositories.BOMStructures.Implementations;
using bom.Managers.Search.Abstractions;
using bom.Managers.Search.Implementations;
using bom.Repositories.Search.Abstractions;
using bom.Repositories.Search.Implementations;
using bom.Managers.BOMStructures;
using Microsoft.Extensions.DependencyInjection;

namespace bom.API.AppSetup
{
    public static class AppSetup
    {
        public static void RegisterDependency(IServiceCollection services, string connectionString)
        {
            RegisterManagers(services);
            RegisterRepositories(services, connectionString);
        }

        private static void RegisterManagers(IServiceCollection services)
        {
            // ItemMasterSales
            services.AddScoped<IItemMasterSalesManager, ItemMasterSalesManager>();
            

            // ItemMasterRawMaterials
            services.AddScoped<IItemMasterRawMaterialsManager, ItemMasterRawMaterialsManager>();
            

            // SubAssemblies
            services.AddScoped<ISubAssembliesManager, SubAssembliesManager>();
            


            // BOMStructures
            services.AddScoped<IBOMStructuresManager, BOMStructuresManager>();


            // BOMTree
            services.AddScoped<IBOMTreeManager, BOMTreeManager>();
           

            // Search
            services.AddScoped<ISearchManager, SearchManager>();
        }

        private static void RegisterRepositories(IServiceCollection services, string connectionString)
        {
            // ItemMasterSales
            services.AddScoped<IItemMasterSalesRepository>(x => new ItemMasterSalesRepository(connectionString));
            

            // ItemMasterRawMaterials
            services.AddScoped<IItemMasterRawMaterialsRepository>(x => new ItemMasterRawMaterialsRepository(connectionString));
            

            // SubAssemblies
            services.AddScoped<ISubAssembliesRepository>(x => new SubAssembliesRepository(connectionString));
            
            
            // BOMStructures
            services.AddScoped<IBOMStructuresRepository>(x => new BOMStructuresRepository(connectionString));

            // BOMTree
            services.AddScoped<IBOMTreeRepository>(x => new BOMTreeRepository(connectionString));

            // Search
            services.AddScoped<ISearchRepository>(x => new SearchRepository(connectionString));
        }

    }
}

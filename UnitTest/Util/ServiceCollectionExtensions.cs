using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestPlugins.Managers;
using TestPlugins.Plugin;
using TestPlugins.Stores;

namespace UnitTest.Util
{
    public static class ServiceCollectionExtensions
    {
        //复制对应项目 Plugin Init()
        public static void AddUserDefined(this IServiceCollection services, IConfigurationRoot configuration)
        {

//            services.AddDbContextPool<TestDbContext>(options => {
//                options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
//#if DEBUG
//                options.UseLoggerFactory(new EFLoggerFactory());
//#endif
//            });
            services.AddScoped<TestDbContext>();
            services.AddScoped<IUserStores, UserStores>();
            services.AddScoped<UserManager>();
            services.AddScoped<IAdminStores, AdminStores>();
            services.AddScoped<AdminManager>();
        }
    }
}

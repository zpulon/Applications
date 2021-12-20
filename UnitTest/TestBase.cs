using ApiCore.Basic;
using ApiCore.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PluginCore.Basic;
using PluginCore.Plugin;
using System;
using System.Reflection;
using TestPlugins.Plugin;
using UnitTest.Util;
using Xunit;

namespace UnitTest
{
    public class TestBase<TContext> where TContext : DbContext
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly TContext Context;
        public TestBase()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            services.AddSingleton<IConfigurationRoot>(configuration);

            services.AddMvc()
                    .AddViewComponentsAsServices();
            services.AddDbContextPool<TContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
#if DEBUG
                options.UseLoggerFactory(new EFLoggerFactory());
#endif
            });
            services.AddUserDefined(configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            ServiceProvider = services.BuildServiceProvider();
            Context = ServiceProvider.GetRequiredService<TContext>();
            var application = new PluginCoreContext(services)
            {
                ServiceProvider = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider,
                PluginFactory = new DefaultPluginFactory(),
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                AuthUrl = configuration["AuthUrl"],
                ClientID = configuration["ClientID"],
                ClientSecret = configuration["ClientSecret"],

            };
            ICoreServiceCollectionExtensions.ServiceProvider = ServiceProvider;
        }
        public void Dispose()
        {
        }
    }
    /// <summary>
    /// 定义Collection名称，标明使用的Fixture
    /// </summary>
    [CollectionDefinition("UnitTestCollection")]
    public class TestBaseCollection : ICollectionFixture<TestBase<TestDbContext>>
    {

    }
}

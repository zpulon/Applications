using ApiCore.Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PluginCore.Basic;
using PluginCore.Plugin;
using System.Threading.Tasks;
using TestPlugins.Managers;
using TestPlugins.Stores;

namespace TestPlugins.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public class Plugin : PluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override string PluginID
        {
            get
            {
                return "TestPlugins";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string PluginName
        {
            get
            {
                return "测试";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string Description
        {
            get
            {
                return "测试插件";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PluginMessage> Init(PluginCoreContext context)
        {
            //var config = context.Services.BuildServiceProvider().GetService<IConfigurationRoot>();
            //数据库连接默认starup 配置 可以在appsettings.json 配置 其他库config[]

            context.Services.AddDbContextPool<TestDbContext>(options => {
                options.UseSqlServer(context.ConnectionString);
#if DEBUG
                options.UseLoggerFactory(new EFLoggerFactory());
#endif
            });
            context.Services.AddScoped<UserManager>();
            context.Services.AddScoped<IUserStores, UserStores>();
            context.Services.AddScoped<AdminManager>();
            context.Services.AddScoped<IAdminStores, AdminStores>();
            return base.Init(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public override Task<PluginMessage> Start(PluginCoreContext context)
        {
          
            return base.Start(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PluginMessage> Stop(PluginCoreContext context)
        {
            return base.Stop(context);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestPlugins.Models;
using TestPlugins.Plugin;

namespace TestPlugins.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminStores : IAdminStores
    {
        /// <summary>
        /// 
        /// </summary>
        protected TestDbContext context { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public AdminStores(TestDbContext applicationDbContext)
        {
            context = applicationDbContext;
            oS_Admins = applicationDbContext.OS_Admins;

        }
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<OS_Admin> oS_Admins { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<OS_Admin> QueryAdmin()
        {
            return oS_Admins.AsNoTracking();
        }
    }
}

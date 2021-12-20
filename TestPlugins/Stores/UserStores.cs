using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestPlugins.Models;
using TestPlugins.Plugin;

namespace TestPlugins.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public class UserStores : IUserStores
    {
        /// <summary>
        /// 
        /// </summary>
        protected TestDbContext context { get; }
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<OS_User> oS_Users { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public UserStores(TestDbContext applicationDbContext)
        {
            context = applicationDbContext;
            oS_Users = applicationDbContext.OS_Users;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<OS_User> QueryUser()
        {
            return oS_Users.AsNoTracking();
        }
    }
}

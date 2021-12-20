using ApiCore.Stores;
using System.Linq;
using TestPlugins.Models;

namespace TestPlugins.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IUserStores: IRepository<OS_User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<OS_User> QueryUser();
        /// <summary>
        /// 
        /// </summary>
        IQueryable<OS_User> oS_Users { get; }
    }
}

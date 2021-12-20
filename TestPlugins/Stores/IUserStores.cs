using System.Linq;
using TestPlugins.Models;

namespace TestPlugins.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserStores
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

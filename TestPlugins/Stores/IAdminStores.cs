using System.Linq;
using TestPlugins.Models;

namespace TestPlugins.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdminStores
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<OS_Admin> QueryAdmin();
        /// <summary>
        /// 
        /// </summary>
        IQueryable<OS_Admin> oS_Admins { get; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestPlugins.Models;
using TestPlugins.Stores;

namespace TestPlugins.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminManager
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IAdminStores _iadminStores;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminStores"></param>
        public AdminManager(IAdminStores adminStores)
        {
            _iadminStores = adminStores;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<OS_Admin> Get_AdminById(long id)
        {
            return _iadminStores.QueryAdmin().Where(z => z.Id == id).Select(z => new OS_Admin { Id = z.Id, Name = z.Name }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<OS_Admin> Get_AdminByPredicate(Expression<Func<OS_Admin, bool>> predicate)
        {
            return _iadminStores.QueryAdmin().Where(predicate).FirstOrDefaultAsync();
        }
    }
}

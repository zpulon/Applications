using ApiCore.Basic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestPlugins.Dto.Response;
using TestPlugins.Models;
using TestPlugins.Stores;

namespace TestPlugins.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserManager
    {
        private readonly IUserStores _iuserStores;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userStores"></param>
        /// <param name="mapper"></param>
        public UserManager(IUserStores userStores, IMapper mapper)
        {
            _iuserStores = userStores;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<OS_User> Get_UserByIdAsync(long id)
        {
            return _iuserStores.QueryUser().Where(z => z.Id == id).Select(z => new OS_User { Id = z.Id, Name = z.Name, SchoolName = z.SchoolName, GraduationYear = z.GraduationYear, StudentNumber = z.StudentNumber, ClassName = z.ClassName }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<OS_User> Get_UserByPredicateAsync(Expression<Func<OS_User, bool>> predicate)
        {
            return _iuserStores.QueryUser().Where(predicate).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<PagingResponseMessage<UserResponse>> Get_UserListAsync()
        {
            PagingResponseMessage<UserResponse> response = new PagingResponseMessage<UserResponse> { Extension = new List<UserResponse>() };
            var user= _iuserStores.QueryUser();
            response.TotalCount =await user.CountAsync();
            var result =await user.Skip(0).Take(10).ToListAsync();
            response.Extension = _mapper.Map<List<UserResponse>>(result);
            return response;
        }
    }
}

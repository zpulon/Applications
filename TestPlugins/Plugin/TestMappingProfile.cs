using AutoMapper;
using TestPlugins.Dto.Response;
using TestPlugins.Models;

namespace TestPlugins.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public class TestMappingProfile:Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public TestMappingProfile()
        {
            CreateMap<OS_User, UserResponse>();
        }
    }
}

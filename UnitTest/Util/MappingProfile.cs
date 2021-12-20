using AutoMapper;
using TestPlugins.Dto.Response;
using TestPlugins.Models;

namespace UnitTest.Util
{
    public class MappingProfile : Profile
    {
        //复制对应的项目*MappingProfile
        public MappingProfile()
        {
            CreateMap<OS_User, UserResponse>();
        }
    }
}

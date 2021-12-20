using Microsoft.Extensions.DependencyInjection;
using TestPlugins.Managers;
using TestPlugins.Plugin;
using Xunit;

namespace UnitTest.TestModel
{
    /// <summary>
    /// 单元测试
    /// </summary>
    [Collection("UnitTestCollection")]
    public class UserListTest
    {
        private readonly UserManager _userManager;
        private readonly TestBase<TestDbContext> _testBase;
        public UserListTest(TestBase<TestDbContext> testBase)
        {
            _testBase = testBase;
            _userManager = _testBase.ServiceProvider.GetRequiredService<UserManager>();
        }
        [Fact(DisplayName = "用户列表测试")]
        public async void TestGetUserList()
        {
            var result=await _userManager.Get_UserListAsync();
           Assert.True(result.Extension.Count>0, "是否查询成功");
        }
    }
}

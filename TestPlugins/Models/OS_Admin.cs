using System;
using System.ComponentModel;

namespace TestPlugins.Models
{
    public class OS_Admin
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public long Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>		
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 状态#1正常，2锁定
        /// </summary>		
        public AdminStatus Status { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>		
        public string Mobile { get; set; }

        /// <summary>
        /// 账户
        /// </summary>		
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>		
        public string Email { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>		
        public long RoleId { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>		
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人编号
        /// </summary>		
        public long UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>		
        public bool DeleteState { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>		
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>		
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 教师编号
        /// </summary>
        public long TeacherId { get; set; }
    }
    /// <summary>
    /// 管理员状态
    /// </summary>
    public enum AdminStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Lock = 2
    }
}

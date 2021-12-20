using Microsoft.EntityFrameworkCore;
using TestPlugins.Models;

namespace TestPlugins.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public class TestDbContext  : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<OS_User> OS_Users { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<OS_Admin> OS_Admins { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OS_User>(b =>
            {
                b.ToTable("OS_User");
            });
            modelBuilder.Entity<OS_Admin>(b =>
            {
                b.ToTable("OS_Admin");
            });

        }
    }
}

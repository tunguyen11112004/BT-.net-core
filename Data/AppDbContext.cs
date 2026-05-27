using BTNetcore.Models;
using Microsoft.EntityFrameworkCore;

namespace BTNetcore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Course> Courses { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình quan hệ Một - Nhiều
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Category)           // Khóa học có một Danh mục
            .WithMany(cat => cat.Courses)      // Danh mục có nhiều Khóa học
            .HasForeignKey(c => c.CategoryId)  // Chỉ định khóa ngoại là CategoryId
            .OnDelete(DeleteBehavior.Restrict); // Ngăn xóa Danh mục nếu đang có Khóa học tham chiếu đến
    }
}
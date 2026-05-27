using BTNetcore.Models;
using Microsoft.EntityFrameworkCore;

namespace BTNetcore.Data;

public class DbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Đảm bảo Database đã được tạo thông qua Migrations
            context.Database.Migrate();

            // 1. Thực hiện Reset dữ liệu
            /*ClearData(context);*/

            // 2. Thực hiện Seeding dữ liệu mới
            SeedCategory(context);
            SeedCourse(context);
        }
    }
    public static void SeedCategory(AppDbContext context)
    {
        if (context.Categories.Any())
        {
            return;
        }
        context.Categories.AddRange(new List<Category> 
        {
            new Category {Name = "Ai",  Description="Ai cơ bản"},
            new Category {Name = "Python",  Description="Python cơ bản"},
            new Category {Name = "Node JS",  Description="Node JS cơ bản"},
            new Category {Name = "Java",  Description="Java cơ bản"},
            new Category {Name = "Java script",  Description="Java script cơ bản"}
        });
        context.SaveChanges();
    }
    
    public static void SeedCourse(AppDbContext context)
    {
        if (context.Courses.Any())
        {
            return;
        }
        context.Courses.AddRange(new List<Course> 
        {
            new Course {Title = "khóa học 1",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 2",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 0},
            new Course {Title = "khóa học 3",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 4",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 0},
            new Course {Title = "khóa học 5",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 6",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 0},
            new Course {Title = "khóa học 7",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 8",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 9",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 10",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 0},
            new Course {Title = "khóa học 11",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 12",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 13",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 14",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 0},
            new Course {Title = "khóa học 15",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 16",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 17",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 18",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 0},
            new Course {Title = "khóa học 19",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
            new Course {Title = "khóa học 20",  Description="Ai cơ bản", Content = "hiii", ImageUrl = "", StartDate =  DateTime.Today, EndDate = DateTime.Today.AddDays(7), Status = 1},
        });
        context.SaveChanges();
    }
}
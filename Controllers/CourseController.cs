using BTNetcore.Data;
using BTNetcore.Helpers;
using BTNetcore.Models;
using BTNetcore.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTNetcore.Controllers;

public class CourseController : Controller
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context) => _context = context;
    
    
    // GET
    public async Task<IActionResult> Index(CourseFilterViewModel filter, int? pageIndex)
    {
        var query = _context.Courses.Include(c => c.Category).AsQueryable();

        // 1. Logic Lọc
        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            /*query = query.Where(u => u.Title.Contains(filter.Keyword));*/
            var keyword = $"%{filter.Keyword.Trim()}%"; // Tạo chuỗi dạng %từ_khóa%
            query = query.Where(u => EF.Functions.Like(u.Title, keyword));
        }
        
        // 2. Logic Thời gian
        if (!string.IsNullOrEmpty(filter.DateRange))
        {
            var dates = filter.DateRange.Split('-').Select(d => d.Trim()).ToArray();
            if (dates.Length == 2)
            {
                var startDate = DateTime.ParseExact(dates[0], "MM/dd/yyyy", null);
                var endDate = DateTime.ParseExact(dates[1], "MM/dd/yyyy", null);
                query = query.Where(u => u.StartDate >= startDate && u.StartDate <= endDate);
            }
        }

        // 3. Logic Sắp xếp
        query = filter.SortOrder switch
        {
            "name_desc" => query.OrderByDescending(u => u.Title),
            "date_asc" => query.OrderBy(u => u.StartDate),
            "date_desc" => query.OrderByDescending(u => u.StartDate),
            _ => query.OrderBy(u => u.Title),
        };
        
        // 4. Thực thi Phân trang
        int pageSize = 10; // Số lượng bản ghi trên mỗi trang
        
        /*filter.Courses = await query.ToListAsync();
        
        filter.Categories = await _context.Courses
            .Include(c => c.Category)
            .ToListAsync();*/
        
        filter.Courses = await PaginatedList<Course>.CreateAsync(query.AsNoTracking(), pageIndex ?? 1, pageSize);

        // Nếu bạn vẫn muốn giữ cái dòng lấy tất cả khóa học gốc phân trang:
        var allCoursesQuery = _context.Courses.Include(c => c.Category).AsQueryable();
        filter.Categories = await PaginatedList<Course>.CreateAsync(allCoursesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
        return View(filter);
    }
    

    public async Task<IActionResult> Create()
    {
        // Lấy danh sách ID và Name để đổ vào Dropdown
        var categories = await _context.Categories.ToListAsync();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
    
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid) return View(course);
        _context.Add(course);
        var sanitizer = new HtmlSanitizer();
        string cleanHtml = sanitizer.Sanitize(course.Content);

        course.Content = cleanHtml;
        _context.Add(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
        var course = await _context.Courses.FindAsync(id);
        return course == null ? NotFound() : View(course);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Course course)
    {
        if (!ModelState.IsValid) return View(course);
        var sanitizer = new HtmlSanitizer();
        string cleanHtml = sanitizer.Sanitize(course.Content);

        course.Content = cleanHtml;
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken] // Nên bổ sung để bảo mật (Cần gửi kèm Token nếu dùng AJAX phức tạp hơn)
    public async Task<IActionResult> Delete(List<int> ids)
    {
        /*var course = await _context.Courses.FindAsync(id);*/
        /*if (course != null)
        {
            course.Status = 0;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));*/
        if (ids == null || ids.Count == 0)
        {
            return Json(new { success = false, message = "Không có tài khoản nào được chọn." });
        }
        
        try
        {
            
            var courseToDelete = await _context.Courses.Where(u => ids.Contains(u.Id)).ToListAsync();

            if (courseToDelete.Any())
            {
                foreach(var course in courseToDelete) {
                    course.Status = 0;
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = $"Đã kết thúc khóa học thành công {courseToDelete.Count} khóa học." });
            }
            return Json(new { success = false, message = "Không tìm thấy dữ liệu phù hợp." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }
}
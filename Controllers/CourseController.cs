using BTNetcore.Data;
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
    public async Task<IActionResult> Index(CourseFilterViewModel filter)
    {
        var query = _context.Courses.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            /*query = query.Where(u => u.Title.Contains(filter.Keyword));*/
            var keyword = $"%{filter.Keyword.Trim()}%"; // Tạo chuỗi dạng %từ_khóa%
            query = query.Where(u => EF.Functions.Like(u.Title, keyword));
        }

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

        query = filter.SortOrder switch
        {
            "name_desc" => query.OrderByDescending(u => u.Title),
            "date_asc" => query.OrderBy(u => u.StartDate),
            "date_desc" => query.OrderByDescending(u => u.StartDate),
            _ => query.OrderBy(u => u.Title),
        };
        
        filter.Courses = await query.ToListAsync();
        
        filter.Categories = await _context.Courses
            .Include(c => c.Category)
            .ToListAsync();
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
    public async Task<IActionResult> Delete(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            course.Status = 0;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
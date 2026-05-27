using BTNetcore.Data;
using BTNetcore.Models;
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
    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses
            .Include(c => c.Category) // Yêu cầu lấy kèm dữ liệu Category
            .ToListAsync();
        return View(courses);
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
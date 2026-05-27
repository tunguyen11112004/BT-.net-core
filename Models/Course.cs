using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTNetcore.Models;

public class Course
{
    /*[key]*/
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [MinLength(10)]
    public string Title  { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    public string? Content  { get; set; } = string.Empty;
    
    public string? ImageUrl { get; set; } 

    [NotMapped] // Không tạo cột này trong Database
    public IFormFile? ImageFile { get; set; } 
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal Price { get; set; }
    
    public DateTime StartDate  { get; set; }
    
    public DateTime EndDate  { get; set; }

    public int Status { get; set; } = 1;

    // Thuộc tính khóa ngoại (Foreign Key)
    public int CategoryId { get; set; }

    // Thuộc tính điều hướng: Một khóa học thuộc về một danh mục
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
}
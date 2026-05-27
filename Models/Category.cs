using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BTNetcore.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Tên danh mục không được để trống")]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    public virtual ICollection<Course> Courses { get; set; } = new Collection<Course>();
}
using BTNetcore.Helpers;
using BTNetcore.Models;

namespace BTNetcore.ViewModels;

public class CourseFilterViewModel
{
    public string? Keyword { get; set; }
    public string? DateRange { get; set; } // Dạng "MM/DD/YYYY - MM/DD/YYYY"
    public string? SortOrder { get; set; }
    
    public int? PageIndex { get; set; }
    
    public PaginatedList<Course> Courses { get; set; }
    
    public PaginatedList<Course> Categories { get; set; }
    /*public IEnumerable<Course> Courses { get; set; } = new List<Course>();*/
    /*public IEnumerable<Course> Categories { get; set; } = new List<Course>();*/   
}
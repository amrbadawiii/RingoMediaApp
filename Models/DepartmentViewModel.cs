using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RingoMediaApp.Models;
public class DepartmentViewModel
{
    public int Id { get; set; }
    [Required]
    public string DepartmentName { get; set; }
    public IFormFile DepartmentLogo { get; set; }
    public string? DepartmentLogoPath { get; set; }
    public int? ParentId { get; set; }
    public IEnumerable<SelectListItem>? ParentDepartments { get; set; }
}
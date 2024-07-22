using System.ComponentModel.DataAnnotations;

namespace RingoMediaApp.Models;
public class DepartmentModel
{
    public int Id { get; set; }
    [Required]
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentLogo { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public DepartmentModel ParentDepartment { get; set; }
    public ICollection<DepartmentModel> SubDepartments { get; set; } = new List<DepartmentModel>();

}

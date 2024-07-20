namespace RingoMediaApp.Models;
public class Department
{
    public int Id { get; set; }
    public string DepartmentName { get; set; }
    public string DepartmentLogo { get; set; }
    public int? ParentId { get; set; }
    public Department ParentDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();

}

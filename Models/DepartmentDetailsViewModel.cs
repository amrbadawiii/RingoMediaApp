using RingoMediaApp.Models;

public class DepartmentDetailsViewModel
{
    public DepartmentModel Department { get; set; }
    public IEnumerable<DepartmentModel> SubDepartments { get; set; }
}

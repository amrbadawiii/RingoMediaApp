using System.ComponentModel.DataAnnotations;

namespace RingoMediaApp.Models;
public class ReminderViewModel
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateTime { get; set; }
    public string Email { get; set; }
}

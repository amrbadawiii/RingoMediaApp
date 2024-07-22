using System.ComponentModel.DataAnnotations;

namespace RingoMediaApp.Models;
public class ReminderModel
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public DateTime DateTime { get; set; }
}

namespace RingoMediaApp.Models;
public class ReminderModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime DateTime { get; set; }
    public string Email { get; set; }
    public bool IsSent { get; set; }
}

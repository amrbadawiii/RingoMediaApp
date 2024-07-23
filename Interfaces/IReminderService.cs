using RingoMediaApp.Models;

namespace RingoMediaApp.Interfaces;
public interface IReminderService
{
    Task<List<ReminderModel>> GetPendingRemindersAsync();
    Task MarkAsSentAsync(ReminderModel reminder);
}


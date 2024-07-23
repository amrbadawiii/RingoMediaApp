using Microsoft.EntityFrameworkCore;
using RingoMediaApp.Database;
using RingoMediaApp.Interfaces;
using RingoMediaApp.Models;

namespace RingoMediaApp.Services;

public class ReminderService : IReminderService
{
    private readonly ApplicationDbContext _context;

    public ReminderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReminderModel>> GetPendingRemindersAsync()
    {
        var now = DateTime.Now;
        return await _context.Reminders
            .Where(r => r.DateTime <= now && !r.IsSent)
            .ToListAsync();
    }

    public async Task MarkAsSentAsync(ReminderModel reminder)
    {
        reminder.IsSent = true;
        _context.Update(reminder);
        await _context.SaveChangesAsync();
    }
}

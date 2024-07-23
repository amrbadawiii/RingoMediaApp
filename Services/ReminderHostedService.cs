using RingoMediaApp.Interfaces;

namespace RingoMediaApp.Services;

public class ReminderHostedService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<ReminderHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ReminderHostedService(ILogger<ReminderHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SendReminders, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private async void SendReminders(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var reminders = await reminderService.GetPendingRemindersAsync();

            foreach (var reminder in reminders)
            {
                string to = reminder.Email;
                string subject = "Reminder: " + reminder.Title;
                string content = "This is a reminder for: " + reminder.Title;
                
                await emailSender.SendEmailAsync(to, subject, content);
                await reminderService.MarkAsSentAsync(reminder);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

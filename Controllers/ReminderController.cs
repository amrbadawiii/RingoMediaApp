using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMediaApp.Database;
using RingoMediaApp.Models;

namespace RingoMediaApp.Controllers;
public class ReminderController : Controller
{
    ApplicationDbContext _context;
    public ReminderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Reminders.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var reminder = await _context.Reminders
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reminder == null)
            return NotFound();

        return View(reminder);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReminderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var reminder = new ReminderModel
            {
                Title = model.Title,
                DateTime = model.DateTime,
                Email = model.Email,
            };

            _context.Add(reminder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();
        
        var reminder = await _context.Reminders.FindAsync(id);
        
        if (reminder == null)
            return NotFound();

        var model = new ReminderViewModel
        {
            Id = reminder.Id,
            Title = reminder.Title,
            Email = reminder.Email,
            DateTime = reminder.DateTime
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReminderViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var reminder = await _context.Reminders.FindAsync(id);
                if (reminder == null)
                    return NotFound();

                reminder.Title = model.Title;
                reminder.DateTime = model.DateTime;

                _context.Update(reminder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReminderExists(model.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var reminder = await _context.Reminders
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reminder == null)
            return NotFound();

        return View(reminder);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
            return NotFound();

        var reminder = await _context.Reminders.FindAsync(id);
        if (reminder == null)
            return NotFound();

        _context.Reminders.Remove(reminder);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool ReminderExists(int id)
    {
        return _context.Reminders.Any(e => e.Id == id);
    }
}

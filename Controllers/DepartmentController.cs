using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RingoMediaApp.Database;
using RingoMediaApp.Models;

namespace RingoMediaApp.Controllers;
public class DepartmentController : Controller
{
    ApplicationDbContext _context;

    public DepartmentController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var departments = await _context.Departments
            .Include(d => d.SubDepartments)
            .ToListAsync();

        return View(departments);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if(id == null)
            return NotFound();

        var department = await _context.Departments
            .Include(d => d.SubDepartments)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
            return NotFound();

        var subDepartments = await _context.Departments
        .Where(d => d.ParentId == id)
        .ToListAsync();

        var viewModel = new DepartmentDetailsViewModel
        {
            Department = department,
            SubDepartments = subDepartments
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var departments = await _context.Departments.ToListAsync();

        var viewModel = new DepartmentViewModel
        {
            ParentDepartments = new SelectList(departments, "Id", "DepartmentName")
        };
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var department = new DepartmentModel
            {
                DepartmentName = model.DepartmentName,
                ParentId = model.ParentId
            };

            if (model.DepartmentLogo != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(model.DepartmentLogo.FileName);
                string extension = Path.GetExtension(model.DepartmentLogo.FileName);
                department.DepartmentLogo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine("wwwroot/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.DepartmentLogo.CopyToAsync(fileStream);
                }
                department.DepartmentLogo = Path.Combine("images", fileName);
            }
            
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", model.ParentId);
        return View(model);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return NotFound();

        var departments = await _context.Departments.ToListAsync();

        var viewModel = new DepartmentViewModel
        {
            Id = department.Id,
            DepartmentName = department.DepartmentName,
            ParentId = department.ParentId,
            ParentDepartments = new SelectList(departments, "Id", "DepartmentName")
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartmentViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                department.DepartmentName = model.DepartmentName;
                department.ParentId = model.ParentId;

                if (model.DepartmentLogo != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.DepartmentLogo.FileName);
                    string extension = Path.GetExtension(model.DepartmentLogo.FileName);
                    department.DepartmentLogo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine("wwwroot/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.DepartmentLogo.CopyToAsync(fileStream);
                    }
                    department.DepartmentLogo = Path.Combine("images", fileName); ;
                }

                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(model.Id))
                    return NotFound();
                else
                    throw;
                
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", model.ParentId);
        return View(model);

    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();
        
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
            return NotFound();

        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var department = await _context.Departments
        .Include(d => d.SubDepartments)
        .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
            return NotFound();

        foreach (var subDepartment in department.SubDepartments)
        {
            subDepartment.ParentId = null;
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool DepartmentExists(int id)
    {
        return _context.Departments.Any(d => d.Id == id);
    }
}

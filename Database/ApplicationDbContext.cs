using Microsoft.EntityFrameworkCore;
using RingoMediaApp.Models;

namespace RingoMediaApp.Database;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<DepartmentModel> Departments { get; set; }
    public DbSet<ReminderModel> Reminders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DepartmentModel>()
            .HasOne(d => d.ParentDepartment)
            .WithMany(d => d.SubDepartments)
            .HasForeignKey(d => d.ParentId);

        base.OnModelCreating(modelBuilder);
    }
}

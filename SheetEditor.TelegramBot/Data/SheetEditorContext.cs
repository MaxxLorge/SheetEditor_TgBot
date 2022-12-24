using Microsoft.EntityFrameworkCore;
using SheetEditor.Data.Entities;

namespace SheetEditor.Data;

public class SheetEditorContext : DbContext
{
    public SheetEditorContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(SheetEditorContext).Assembly);
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Spreadsheet> Spreadsheets { get; set; }
}
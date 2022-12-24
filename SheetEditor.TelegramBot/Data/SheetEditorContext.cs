using Microsoft.EntityFrameworkCore;
using SheetEditor.Data.Entities;

namespace SheetEditor.Data;

public class SheetEditorContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("memory");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Spreadsheet> Spreadsheets { get; set; }
}
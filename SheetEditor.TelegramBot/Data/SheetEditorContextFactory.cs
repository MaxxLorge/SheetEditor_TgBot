using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SheetEditor.Data;

public class SheetEditorContextFactory : IDbContextFactory<SheetEditorContext>,
    IDesignTimeDbContextFactory<SheetEditorContext>
{
    public SheetEditorContext CreateDbContext(string[] args)
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("secrets.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<SheetEditorContext>();
        optionsBuilder.UseNpgsql(configurationRoot["ConnectionString"]);
        return new SheetEditorContext(optionsBuilder.Options);
    }

    public SheetEditorContext CreateDbContext()
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("secrets.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<SheetEditorContext>();
        optionsBuilder.UseNpgsql(configurationRoot["ConnectionString"]);
        return new SheetEditorContext(optionsBuilder.Options);
    }
}
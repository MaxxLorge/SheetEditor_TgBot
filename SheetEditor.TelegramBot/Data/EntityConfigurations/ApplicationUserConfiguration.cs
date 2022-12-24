using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SheetEditor.Data.Entities;

namespace SheetEditor.Data.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasMany(e => e.Spreadsheets)
            .WithOne()
            .HasForeignKey(e => e.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.TelegramId);
    }
}
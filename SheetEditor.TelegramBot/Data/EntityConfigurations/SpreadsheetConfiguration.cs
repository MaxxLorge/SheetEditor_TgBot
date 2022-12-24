using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SheetEditor.Data.Entities;

namespace SheetEditor.Data.EntityConfigurations;

public class SpreadsheetConfiguration : IEntityTypeConfiguration<Spreadsheet>
{
    public void Configure(EntityTypeBuilder<Spreadsheet> builder)
    {
        builder.HasOne(e => e.ApplicationUser)
            .WithMany()
            .HasForeignKey(e => e.ApplicationUserId);
    }
}
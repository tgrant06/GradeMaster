using GradeMaster.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GradeMaster.DataAccess.Configurations;

public class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("Colors");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedNever();

        builder.Property(c => c.Name).HasColumnName("Name").IsUnicode().HasMaxLength(125).IsRequired().UseCollation("NOCASE");
        builder.Property(c => c.Symbol).HasColumnName("Symbol").IsUnicode().HasMaxLength(64).IsRequired().UseCollation("NOCASE");

        builder.HasMany(c => c.Notes).WithOne(n => n.Color).HasForeignKey(n => n.ColorId).OnDelete(DeleteBehavior.Cascade).IsRequired();

        builder.HasData(new List<Color>
        {
            new Color { Id = 1, Name = "Gray", Symbol = "🔘"},
            new Color { Id = 2, Name = "Red", Symbol = "🔴" },
            new Color { Id = 3, Name = "Orange", Symbol = "🟠" },
            new Color { Id = 4, Name = "Yellow", Symbol = "🟡" },
            new Color { Id = 5, Name = "Green", Symbol = "🟢" },
            new Color { Id = 6, Name = "Blue", Symbol = "🔵" },
            new Color { Id = 7, Name = "Purple", Symbol = "🟣" },
            new Color { Id = 8, Name = "Brown", Symbol = "🟤" },
            new Color { Id = 9, Name = "Black", Symbol = "⚫" },
            new Color { Id = 10, Name = "White", Symbol = "⚪" }
        });
    }
}
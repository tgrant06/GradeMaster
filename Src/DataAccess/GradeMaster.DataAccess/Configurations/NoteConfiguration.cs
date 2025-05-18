using GradeMaster.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GradeMaster.DataAccess.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        builder.Property(n => n.Title).HasColumnName("Title").IsUnicode().HasMaxLength(255).IsRequired().UseCollation("NOCASE");
        builder.Property(n => n.Content).HasColumnName("Content").IsUnicode().HasMaxLength(10_000).IsRequired(false).UseCollation("NOCASE");

        builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
        builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

        builder.Property(n => n.IsPinned).HasColumnName("IsPinned").IsRequired();
        builder.Property(n => n.IsArchived).HasColumnName("IsArchived").IsRequired();

        builder.HasOne(n => n.Color).WithMany(c => c.Notes).HasForeignKey(n => n.ColorId).OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}
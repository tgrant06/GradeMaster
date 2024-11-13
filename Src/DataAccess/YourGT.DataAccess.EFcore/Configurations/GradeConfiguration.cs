using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourGT.Shared.Entities;

namespace YourGT.DataAccess.EFCore.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        builder.Property(g => g.Value).HasColumnName("Value").HasColumnType("decimal(3, 2)").IsRequired(); // grade (format 5.55) Swiss system
        builder.Property(g => g.Description).HasColumnName("Description").IsUnicode().HasMaxLength(2500).IsRequired(false);
        builder.Property(g => g.CreatedAt).HasColumnName("CreatedAt").IsRequired();

        builder.HasOne(g => g.Weight).WithMany(w => w.Grades).HasForeignKey(g => g.WeightId).OnDelete(DeleteBehavior.Cascade).IsRequired();

        builder.HasOne(g => g.Subject).WithMany(s => s.Grades).HasForeignKey(g => g.SubjectId).OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}
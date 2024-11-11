using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourGT.Shared.Entities;

namespace YourGT.DataAccess.EFCore.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        builder.Property(e => e.Value).HasColumnName("Value").IsRequired();
        builder.Property(e => e.Description).HasColumnName("Description").IsUnicode().HasMaxLength(2500).IsRequired(false);
        //builder.Property(e => e.Semesters).HasColumnName("Semesters").IsRequired();

        //builder.Property(e => e.Completed).HasColumnName("Completed").IsRequired();

        //builder.HasMany(e => e.Subjects).WithOne(s => s.Education).HasForeignKey(s => s.EducationId).IsRequired(false);
    }
}
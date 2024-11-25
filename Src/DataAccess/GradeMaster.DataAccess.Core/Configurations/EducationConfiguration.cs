using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GradeMaster.Shared.Core.Entities;

namespace GradeMaster.DataAccess.Core.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{ 
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("Educations");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasColumnName("Name").IsUnicode().HasMaxLength(255).IsRequired();
        builder.Property(e => e.Description).HasColumnName("Description").IsUnicode().HasMaxLength(2500).IsRequired(false);
        builder.Property(e => e.Semesters).HasColumnName("Semesters").IsRequired();

        builder.Property(e => e.Completed).HasColumnName("Completed").IsRequired();

        builder.HasMany(e => e.Subjects).WithOne(s => s.Education).HasForeignKey(s => s.EducationId).OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}
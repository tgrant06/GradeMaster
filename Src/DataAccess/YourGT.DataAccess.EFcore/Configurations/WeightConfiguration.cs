using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YourGT.Shared.Entities;

namespace YourGT.DataAccess.EFCore.Configurations;

public class WeightConfiguration : IEntityTypeConfiguration<Weight>
{
    public void Configure(EntityTypeBuilder<Weight> builder)
    {
        builder.ToTable("Weights");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        builder.Property(w => w.Name).HasColumnName("Name").IsUnicode().HasMaxLength(250).IsRequired();
        builder.Property(w => w.Value).HasColumnName("Value").HasColumnType("decimal(7, 5)").IsRequired();

        builder.HasMany(w => w.Grades).WithOne(g => g.Weight).HasForeignKey(g => g.WeightId).OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}
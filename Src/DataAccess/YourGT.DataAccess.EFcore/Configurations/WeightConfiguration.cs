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
        builder.Property(w => w.Id).HasColumnName("Id").ValueGeneratedNever(); //.ValueGeneratedOnAdd();

        builder.Property(w => w.Name).HasColumnName("Name").IsUnicode().HasMaxLength(100).IsRequired();
        builder.Property(w => w.Value).HasColumnName("Value").HasColumnType("decimal(7, 5)").IsRequired();

        builder.HasMany(w => w.Grades).WithOne(g => g.Weight).HasForeignKey(g => g.WeightId).OnDelete(DeleteBehavior.Cascade).IsRequired();

        builder.HasData(new List<Weight>
        {
            // common ones:
            new Weight {Id = 1, Name = "100%", Value = 1},
            new Weight {Id = 2, Name = "75%", Value = 0.75m},
            new Weight {Id = 3, Name = "66.7%", Value = 0.66667m},
            new Weight {Id = 4, Name = "50%", Value = 0.5m},
            new Weight {Id = 5, Name = "33.3%", Value = 0.33334m},
            new Weight {Id = 6, Name = "25%", Value = 0.25m},
            new Weight {Id = 7, Name = "20%", Value = 0.2m},
            // less common ones:
            new Weight {Id = 8, Name = "90%", Value = 0.9m},
            new Weight {Id = 9, Name = "80%", Value = 0.8m},
            new Weight {Id = 10, Name = "60%", Value = 0.6m},
            new Weight {Id = 11, Name = "40%", Value = 0.4m},
            new Weight {Id = 12, Name = "10%", Value = 0.1m},
            // seldom ones:
            new Weight {Id = 13, Name = "1000%", Value = 10},
            new Weight {Id = 14, Name = "500%", Value = 5},
            new Weight {Id = 15, Name = "300%", Value = 3},
            new Weight {Id = 16, Name = "200%", Value = 2}
        });
    }
}
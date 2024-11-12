using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YourGT.DataAccess.EFCore.Configurations;
using YourGT.Shared.Entities;

namespace YourGT.DataAccess.EFCore;

public class YourGTDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    #region DbSets

    public DbSet<Education> Educations
    {
        get; set;
    }

    public DbSet<Grade> Grades
    {
        get; set;
    }

    public DbSet<Subject> Subjects
    {
        get; set;
    }

    public DbSet<Weight> Weights
    {
        get; set;
    }

    #endregion

    public YourGTDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EducationConfiguration());
        modelBuilder.ApplyConfiguration(new GradeConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new WeightConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_configuration.GetConnectionString("Default"))
            .EnableDetailedErrors();
    }
}
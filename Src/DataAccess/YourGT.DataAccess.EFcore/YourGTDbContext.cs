using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YourGT.DataAccess.EFCore.Configurations;
using YourGT.Shared.Entities;

namespace YourGT.DataAccess.EFCore;

public class YourGTDbContext : DbContext
{
    // commented out for now
    private readonly IConfiguration _configuration;

    //private readonly string _connectionString;

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

    public YourGTDbContext(IConfiguration configuration /*string connectionString*/)
    {
        //_connectionString = connectionString;
        // commented out for now
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
        //var connectionString = _configuration.GetConnectionString("Default");
        options.UseSqlite(_configuration.GetConnectionString("Default"))
            .EnableDetailedErrors();
    }
}
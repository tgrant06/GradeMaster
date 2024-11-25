using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GradeMaster.DataAccess.Core.Configurations;
using GradeMaster.Shared.Core.Entities;

namespace GradeMaster.DataAccess.Core;

public class GradeMasterDbContext : DbContext
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

    public GradeMasterDbContext(IConfiguration configuration /*string connectionString*/)
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
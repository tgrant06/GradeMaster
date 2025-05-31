using Microsoft.EntityFrameworkCore;
using GradeMaster.DataAccess.Configurations;
using GradeMaster.Common.Entities;

namespace GradeMaster.DataAccess;

public class GradeMasterDbContext : DbContext
{
    // commented out for now
    //private readonly IConfiguration _configuration;

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

    public DbSet<Note> Notes
    {
        get; set;
    }

    public DbSet<Color> Colors
    {
        get; set;
    }

    #endregion

    public GradeMasterDbContext(DbContextOptions<GradeMasterDbContext> options)
    : base(options)
    {
        //_connectionString = connectionString;
        // commented out for now
        //_configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EducationConfiguration());
        modelBuilder.ApplyConfiguration(new GradeConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new WeightConfiguration());
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        modelBuilder.ApplyConfiguration(new ColorConfiguration());
    }

    #region OnConfiguring
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //{
    //    var appName = "GradeMaster";
    //    var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName);

    //    if (!Directory.Exists(appDataPath))
    //    {
    //        Directory.CreateDirectory(appDataPath);
    //    }

    //    var connectionString = _configuration.GetConnectionString("Default");
    //    options.UseSqlite($"Data Source={Path.Combine(appDataPath, connectionString)}")
    //                  .EnableDetailedErrors();

    //    Database.Migrate();

    //    //var connectionString = _configuration.GetConnectionString("Default");
    //    //options.UseSqlite(_configuration.GetConnectionString("Default"))
    //    //    .EnableDetailedErrors();
    //}
    #endregion
}
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace YourGT.DataAccess.EFCore;

public class YourGTDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Education> Educations
    {
        get; set;
    }
    public DbSet<Category> CategoryEntries
    {
        get; set;
    }

    //public DbSet<Comment> CommentEntries;

    public WebBlogDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlogConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("Default"))
            .EnableDetailedErrors();
    }
}
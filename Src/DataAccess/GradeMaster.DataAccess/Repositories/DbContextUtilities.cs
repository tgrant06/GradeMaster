using System.Runtime.CompilerServices;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Repositories;

public class DbContextUtilities : IDbContextUtilities
{
    private readonly GradeMasterDbContext _context;

    public DbContextUtilities(GradeMasterDbContext context)
    {
        _context = context;
    }

    public async Task DisconnectFromDbAsync()
    {
        try
        {
            await _context.Database.CloseConnectionAsync();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error disconnecting from database: {ex.Message}");
        }
    }

    public async Task ConnectToDbAsync()
    {
        try
        {
            await _context.Database.OpenConnectionAsync();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error connecting to database: {ex.Message}");
        }
    }

    /// <summary>
    /// Only use before application shutdown.
    /// </summary>
    /// <returns></returns>
    public async Task DisposeDbContextAsync()
    {
        try
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
                await _context.Database.CloseConnectionAsync();
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error disposing DbContext: {ex.Message}");
        }
    }

    //public async Task ChangeConnectionStringAsync(string connectionString)
    //{
    //    try
    //    {
    //        // Close the current connection if it's open
    //        if (_context.Database.CanConnect())
    //        {
    //            await _context.Database.CloseConnectionAsync();
    //        }
    //        // Update the connection string
    //        var optionsBuilder = new DbContextOptionsBuilder<GradeMasterDbContext>();
    //        optionsBuilder.UseSqlite(connectionString);
    //        // Create a new context with the updated options
    //        var newContext = new GradeMasterDbContext(optionsBuilder.Options);
    //        // Replace the current context with the new one
    //        _context.Dispose();
    //        _context = newContext;
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the exception or handle it as needed
    //        Console.WriteLine($"Error changing connection string: {ex.Message}");
    //    }
    //}
}
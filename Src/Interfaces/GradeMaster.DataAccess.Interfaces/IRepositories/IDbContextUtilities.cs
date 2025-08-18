namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface IDbContextUtilities
{
    Task DisconnectFromDbAsync();

    Task ConnectToDbAsync();

    Task DisposeDbContextAsync();
}
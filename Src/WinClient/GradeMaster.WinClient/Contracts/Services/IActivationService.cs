namespace YourGT.WinClient.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}

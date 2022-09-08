using System.Threading.Tasks;

namespace Chaos.Services.Security.Abstractions;

public interface ICredentialManager
{
    Task ChangePasswordAsync(string name, string oldPassword, string newPassword);
    Task SaveNewCredentialsAsync(string name, string password);
    Task<bool> ValidateCredentialsAsync(string name, string password);
}
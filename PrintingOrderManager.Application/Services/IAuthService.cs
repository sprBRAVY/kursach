using PrintingOrderManager.Core.Entities;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<User?> AuthenticateAsync(string email, string password);
    Task<bool> IsEmailTakenAsync(string email);
}
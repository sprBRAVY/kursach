using System.Security.Cryptography;
using System.Text;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private static string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations: 100000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32
        );

        // Формат: {iterations}:{salt}:{hash}
        return $"100000:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 3) return false;

        var iterations = int.Parse(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var hash = Convert.FromBase64String(parts[2]);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            outputLength: 32
        );

        return CryptographicOperations.FixedTimeEquals(hash, computedHash);
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        if (await IsEmailTakenAsync(dto.Email))
            return false;

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            Role = "User"
        };

        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return null;

        if (VerifyPassword(password, user.PasswordHash))
            return user;

        return null;
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email) != null;
    }
}
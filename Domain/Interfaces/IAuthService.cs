using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string username, string email, string password);
        Task<(User? user, string token, string refreshToken)> LoginAsync(string email, string password);
        Task<(string newToken, string newRefreshToken)> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> ConfirmEmailAsync(string token);
        Task<bool> SendPasswordResetEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
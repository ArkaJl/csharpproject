using BCrypt.Net;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(IRepositoryWrapper repository, IConfiguration configuration, IEmailService emailService)
        {
            _repository = repository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<User?> RegisterAsync(string username, string email, string password)
        {
            // Проверка существования пользователя
            var existingUser = await _repository.User
                .FindByCondition(u => u.Email == email || u.Username == username)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                return null;

            // Хеширование пароля
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Создание пользователя
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                LastOnline = DateTime.UtcNow,
                IsEmailConfirmed = false,
                EmailConfirmationToken = GenerateRandomToken()
            };

            await _repository.User.Create(user);
            await _repository.SaveAsync();

            // Отправка email для подтверждения
            await _emailService.SendEmailConfirmationAsync(user.Email, user.EmailConfirmationToken);

            return user;
        }

        public async Task<(User? user, string token, string refreshToken)> LoginAsync(string email, string password)
        {
            var user = await _repository.User
                .FindByCondition(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (null, null!, null!);

            // Обновление времени последнего входа
            user.LastOnline = DateTime.UtcNow;
            await _repository.User.Update(user);
            await _repository.SaveAsync();

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Сохранение refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7));

            await _repository.User.Update(user);
            await _repository.SaveAsync();

            return (user, token, refreshToken);
        }

        public async Task<(string newToken, string newRefreshToken)> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return (null!, null!);

            var user = await _repository.User
                .FindByCondition(u => u.Id.ToString() == userId)
                .FirstOrDefaultAsync();

            if (user == null || user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return (null!, null!);

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7));

            await _repository.User.Update(user);
            await _repository.SaveAsync();

            return (newToken, newRefreshToken);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _repository.User
                .FindByCondition(u => u.EmailConfirmationToken == token)
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            await _repository.User.Update(user);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var user = await _repository.User
                .FindByCondition(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            user.ResetPasswordToken = GenerateRandomToken();
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(2);

            await _repository.User.Update(user);
            await _repository.SaveAsync();

            await _emailService.SendPasswordResetAsync(user.Email, user.ResetPasswordToken);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _repository.User
                .FindByCondition(u => u.ResetPasswordToken == token &&
                                     u.ResetPasswordTokenExpiry > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            await _repository.User.Update(user);
            await _repository.SaveAsync();

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("IsEmailConfirmed", user.IsEmailConfirmed.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15)),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateRandomToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
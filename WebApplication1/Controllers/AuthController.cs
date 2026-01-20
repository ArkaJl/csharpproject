using BusinessLogic.Services;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Contracts.Auth;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterAsync(
                    request.Username,
                    request.Email,
                    request.Password);

                if (user == null)
                    return BadRequest(new { message = "Пользователь с таким email или именем уже существует" });

                return Ok(new { message = "Регистрация успешна. Проверьте email для подтверждения." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации");
                return StatusCode(500, new { message = "Ошибка при регистрации" });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Вариант 1: Явное указание типов
                (User? user, string token, string refreshToken) result =
                    await _authService.LoginAsync(request.Email, request.Password);

                if (result.user == null)
                    return Unauthorized(new { message = "Неверный email или пароль" });

                var response = new AuthResponse
                {
                    Token = result.token,
                    RefreshToken = result.refreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(15),
                    User = new UserDto
                    {
                        Id = result.user.Id,
                        Username = result.user.Username,
                        Email = result.user.Email,
                        AvatarUrl = result.user.AvatarUrl,
                        Coins = result.user.Coins,
                        IsEmailConfirmed = result.user.IsEmailConfirmed,
                        LastOnline = result.user.LastOnline,
                        Status = result.user.Status
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при входе");
                return StatusCode(500, new { message = "Ошибка при входе в систему" });
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // Вариант 1: Явное указание типов
                (string newToken, string newRefreshToken) result =
                    await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

                if (string.IsNullOrEmpty(result.newToken))
                    return Unauthorized(new { message = "Недействительный refresh token" });

                return Ok(new
                {
                    token = result.newToken,
                    refreshToken = result.newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении токена");
                return StatusCode(500, new { message = "Ошибка при обновлении токена" });
            }
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            try
            {
                var result = await _authService.ConfirmEmailAsync(token);

                if (!result)
                    return BadRequest(new { message = "Недействительный или просроченный токен" });

                return Ok(new { message = "Email успешно подтвержден" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при подтверждении email");
                return StatusCode(500, new { message = "Ошибка при подтверждении email" });
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var result = await _authService.SendPasswordResetEmailAsync(request.Email);

                if (!result)
                    return BadRequest(new { message = "Пользователь с таким email не найден" });

                return Ok(new { message = "Инструкции по сбросу пароля отправлены на email" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе сброса пароля");
                return StatusCode(500, new { message = "Ошибка при запросе сброса пароля" });
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(
                    request.Token,
                    request.NewPassword);

                if (!result)
                    return BadRequest(new { message = "Недействительный или просроченный токен" });

                return Ok(new { message = "Пароль успешно изменен" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сбросе пароля");
                return StatusCode(500, new { message = "Ошибка при сбросе пароля" });
            }
        }

        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            return Ok(new { message = "Токен действителен" });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _authService.GetUserByIdAsync(Guid.Parse(userId));
                    if (user != null)
                    {
                        user.RefreshToken = null;
                        user.RefreshTokenExpiryTime = null;
                        // Нужно обновить пользователя в БД
                        // Для этого добавьте метод в IAuthService
                    }
                }

                return Ok(new { message = "Выход выполнен успешно" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выходе");
                return StatusCode(500, new { message = "Ошибка при выходе из системы" });
            }
        }
    }

    // Дополнительные DTO
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = null!;
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
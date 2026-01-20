using Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailConfirmationAsync(string email, string token)
        {
            var clientUrl = _configuration["ClientUrl"] ?? "http://localhost:3000";
            var confirmationLink = $"{clientUrl}/confirm-email?token={Uri.EscapeDataString(token)}";

            var subject = "Подтверждение email адреса";
            var body = $@"
                <h2>Подтверждение email адреса</h2>
                <p>Для завершения регистрации подтвердите ваш email адрес, перейдя по ссылке:</p>
                <p><a href='{confirmationLink}'>Подтвердить email</a></p>
                <p>Или используйте токен: <strong>{token}</strong></p>
                <p>Ссылка действительна 24 часа.</p>
                <br/>
                <p>Если вы не регистрировались на нашем сайте, проигнорируйте это письмо.</p>
            ";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetAsync(string email, string token)
        {
            var clientUrl = _configuration["ClientUrl"] ?? "http://localhost:3000";
            var resetLink = $"{clientUrl}/reset-password?token={Uri.EscapeDataString(token)}";

            var subject = "Сброс пароля";
            var body = $@"
                <h2>Сброс пароля</h2>
                <p>Вы запросили сброс пароля. Для установки нового пароля перейдите по ссылке:</p>
                <p><a href='{resetLink}'>Сбросить пароль</a></p>
                <p>Или используйте токен: <strong>{token}</strong></p>
                <p>Ссылка действительна 2 часа.</p>
                <br/>
                <p>Если вы не запрашивали сброс пароля, проигнорируйте это письмо.</p>
            ";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // ВСЕГДА логируем для разработки
            _logger.LogInformation("=== EMAIL LOG (DEVELOPMENT) ===");
            _logger.LogInformation($"To: {to}");
            _logger.LogInformation($"Subject: {subject}");

            // Извлекаем токен из тела для удобства тестирования
            var tokenMatch = System.Text.RegularExpressions.Regex.Match(body, @"токен:\s*<strong>([^<]+)</strong>");
            if (tokenMatch.Success)
            {
                var token = tokenMatch.Groups[1].Value;
                _logger.LogInformation($"Confirmation Token: {token}");
                _logger.LogInformation($"Confirmation URL: http://localhost:3000/confirm-email?token={token}");
            }

            _logger.LogInformation($"Body Preview: {body.Substring(0, Math.Min(200, body.Length))}...");
            _logger.LogInformation("================================");

            // Проверяем, настроен ли SMTP сервер
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];

            if (string.IsNullOrEmpty(smtpServer) || smtpServer == "smtp.example.com")
            {
                _logger.LogInformation("SMTP not configured. Email logged to console only.");
                return;
            }

            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(
                    "Jlnest Support",
                    emailSettings["SenderEmail"]));

                emailMessage.To.Add(new MailboxAddress("", to));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Получаем значения через GetSection
                var smtpPortString = emailSettings["SmtpPort"];
                var smtpPort = !string.IsNullOrEmpty(smtpPortString) ? int.Parse(smtpPortString) : 587;

                var senderEmail = emailSettings["SenderEmail"];
                var senderPassword = emailSettings["SenderPassword"];

                var enableSslString = emailSettings["EnableSsl"];
                var enableSsl = !string.IsNullOrEmpty(enableSslString) ? bool.Parse(enableSslString) : true;

                _logger.LogInformation($"Attempting to connect to SMTP: {smtpServer}:{smtpPort}");
                _logger.LogInformation($"Using sender email: {senderEmail}");

                // Для Yandex используем StartTls на порту 587
                if (smtpServer.Contains("yandex"))
                {
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                }
                // Для Gmail тоже StartTls
                else if (smtpServer.Contains("gmail"))
                {
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                }
                // Для Mailtrap или других
                else
                {
                    var socketOptions = enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
                    await client.ConnectAsync(smtpServer, smtpPort, socketOptions);
                }

                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {to}");

                // Не выбрасываем исключение дальше, чтобы не ломать регистрацию
                // Просто логируем ошибку и продолжаем
            }
        }
    }
}
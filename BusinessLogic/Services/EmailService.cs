using Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;

namespace BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
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
                <p>Ссылка действительна 2 часа.</p>
                <br/>
                <p>Если вы не запрашивали сброс пароля, проигнорируйте это письмо.</p>
            ";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(
                    "Jlnest Support",
                    _configuration["EmailSettings:SenderEmail"] ?? "noreply@jlnest.com"));

                emailMessage.To.Add(new MailboxAddress("", to));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Для разработки - если нет реального SMTP, просто логируем
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                if (string.IsNullOrEmpty(smtpServer))
                {
                    Console.WriteLine($"DEV MODE: Email to {to}");
                    Console.WriteLine($"Subject: {subject}");
                    Console.WriteLine($"Body: {body}");
                    return;
                }

                await client.ConnectAsync(
                    smtpServer,
                    _configuration.GetValue<int>("EmailSettings:SmtpPort", 587),
                    _configuration.GetValue<bool>("EmailSettings:EnableSsl", true));

                await client.AuthenticateAsync(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Логирование ошибки, но не выбрасывание исключения
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
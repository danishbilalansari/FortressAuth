using System.Net;
using System.Net.Mail;

namespace IdentityServer.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var client = new SmtpClient("smtp.example.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("username", "password"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("noreply@fortressauth.com"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}
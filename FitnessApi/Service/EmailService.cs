using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using FitnessApi.Data;
using FitnessApi.Dto.Meal;
using FitnessApi.IService;
using FitnessApi.Model;
using System.Net.Mail;
using System.Security.Claims;



namespace FitnessApi.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailService(IOptions<EmailSettings> emailSettings, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _emailSettings = emailSettings.Value;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SendEmail(SendEmailModel request)
        {
            var email = CreateEmailMessage(request);
            Send(email, request.To);
        }

        private MimeMessage CreateEmailMessage(SendEmailModel request)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(MailboxAddress.Parse(request.To));
            emailMessage.Subject = request.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Body };

            return emailMessage;
        }

        private async void Send(MimeMessage mailMessage, string sendEmail)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_emailSettings.EmailHost, _emailSettings.EmailPort, SecureSocketOptions.StartTls);
                    client.Authenticate(_emailSettings.EmailUsername, "jizx jtbw bvmn fjkb");
                    client.Send(mailMessage);
                }
                catch
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    //var faildEmail = new FailedToSendEmail
                    //{
                    //    Category = "Payment",
                    //    Email = sendEmail,
                    //    UserId = userId
                    //};
                    //await _db.FailedToSendEmails.AddAsync(faildEmail);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

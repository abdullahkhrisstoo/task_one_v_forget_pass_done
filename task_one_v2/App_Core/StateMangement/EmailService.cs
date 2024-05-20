using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using task_one_v2.App_Core.Mail;


namespace task_one_v2.App_Core.StateMangement
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetLink);


        public class EmailService : IEmailService
        {
            private readonly SmtpSettings _smtpSettings;

            public EmailService(IOptions<SmtpSettings> smtpSettings)
            {
                _smtpSettings = smtpSettings.Value;
            }

            public Task SendPasswordResetEmailAsync(string email, string resetLink)
            {
                return Task.Run(() =>
                {
                    using (var mail = new MailMessage())
                    {
                        mail.From = new MailAddress(_smtpSettings.From);
                        mail.To.Add(email);
                        mail.Subject = "Password Reset";
                        mail.Body = $"Please reset your password by clicking on the link: {resetLink}";
                        mail.IsBodyHtml = true;

                        using (var smtpServer = new SmtpClient(_smtpSettings.Host))
                        {
                            smtpServer.Port = _smtpSettings.Port;
                            smtpServer.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                            smtpServer.EnableSsl = _smtpSettings.EnableSsl;
                            smtpServer.Send(mail);
                        }
                    }
                });
            }
        }

    }
}

using System;
using System.Net;
using System.Net.Mail;

namespace task_one_v2.App_Core.Mail
{
    public static class SendEmail
    {
        public static void Send(string receiver, string title, string body, string attachmentPath)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Ethereal.etherealMail);
                    mail.To.Add(receiver);
                    mail.Subject = title;
                    mail.Body = body;

                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        Attachment attachment = new Attachment(attachmentPath);
                        mail.Attachments.Add(attachment);
                    }
                    using (SmtpClient smtpServer = new SmtpClient(Ethereal.etherealHost))
                    {
                        smtpServer.Port = Ethereal.etherealPort;
                        smtpServer.Credentials = new NetworkCredential(Ethereal.etherealMail, Ethereal.etherealPassword);
                        smtpServer.EnableSsl = true;
                        smtpServer.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
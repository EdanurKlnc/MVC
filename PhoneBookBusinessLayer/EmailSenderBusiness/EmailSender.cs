
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PhoneBookBusinessLayer.EmailSenderBusiness
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string SenderMail => _configuration.GetSection("EmailOptions:SenderMail").Value;
        public string Password => _configuration.GetSection("EmailOptions:Password").Value;
        public string Smtp => _configuration.GetSection("EmailOptions:Smtp").Value;
        public int SmtpPort => Convert.ToInt32(_configuration.GetSection("EmailOptions:SmtpPort").Value);

        public string CCManager => _configuration.GetSection("ProjectManagersEmails").Value;

        private void MailInfoSet(EmailMessage message, out MailMessage mail, out SmtpClient client) //void dışarıya gönderim yapamadığı için out ile dışarıya aktarıyoruz 
        {
            try
            {
                mail = new MailMessage()
                {
                    From = new MailAddress(SenderMail)//proenin emaili
                };
                // mesajın kime gideceğini ekliyoruz
                foreach (var item in message.To)
                {
                    mail.To.Add(item);
                }
                if (message.CC != null)
                {
                    foreach (var item in message.CC)
                    {
                        mail.CC.Add(item);
                    }
                }
                if (message.CC != null)
                {
                    foreach (var item in message.CC)
                    {
                        mail.Bcc.Add(item);
                    }
                }

                foreach(var item in CCManager.ToString().Split(","))
                    {
                    mail.CC.Add(item);
                }
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8; //türkce karakter girebilsin
                mail.SubjectEncoding = Encoding.UTF8;

                client = new SmtpClient(Smtp, SmtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(SenderMail, Password) //emaile girebilmek için kullanıcı adı ve sifresi

                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SendEmail(EmailMessage message)
        {
            try
            {
                MailInfoSet(message, out MailMessage mail, out SmtpClient client);
                client.Send(mail);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            try
            {
                MailInfoSet(message, out MailMessage mail, out SmtpClient client);
                await client.SendMailAsync(mail);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

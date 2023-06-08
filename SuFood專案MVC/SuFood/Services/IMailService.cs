using System.Net.Mail;
using System.Net;
using System.Text;

namespace SuFood.Services
{
    public interface IEmailService
    {
        void SendEmail(string toAddress, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        public void SendEmail(string toAddress, string subject, string body)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress("SuFood2u@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };
            mail.To.Add(new MailAddress(toAddress));

            try
            {
                using (var sm = new SmtpClient("smtp.gmail.com", 587))
                {
                    sm.EnableSsl = true;
                    sm.Credentials = new NetworkCredential("SuFood2u@gmail.com", "okjzbnxgsmkmfwlq");
                    sm.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

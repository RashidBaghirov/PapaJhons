using System.Net.Mail;
using System.Net;

namespace Papa_Jhons.Services
{
    public class EmailService
    {
        public void SendEmail(string recipientEmail, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("papajhons844@gmail.com", "PapaJhons");
                mail.To.Add(new MailAddress(recipientEmail));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("papajhons844@gmail.com", "lgwrwquagxyirjkm");

                    smtp.Send(mail);
                }
            }
        }
    }
}

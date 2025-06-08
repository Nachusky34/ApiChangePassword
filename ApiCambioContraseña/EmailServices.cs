using System.Net;
using System.Net.Mail;

namespace ApiCambioContraseña
{

    public class EmailService
    {
        private readonly string smtpHost = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string emailOrigin = "daretwogo@gmail.com";
        private readonly string keyApp = "jvha xpzz jnjw tnbj";

        public async Task SendEmailAsync(string destination, string head, string body)
        {
            var mensaje = new MailMessage();
            mensaje.From = new MailAddress(emailOrigin);
            mensaje.To.Add(destination);
            mensaje.Subject = head;
            mensaje.Body = body;
            mensaje.IsBodyHtml = true;

            using var smtp = new SmtpClient(smtpHost, smtpPort);
            smtp.Credentials = new NetworkCredential(emailOrigin, keyApp);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mensaje);

            Console.WriteLine("Email has send correctly");
        }
    }
}

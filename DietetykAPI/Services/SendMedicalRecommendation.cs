using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace DietetykAPI.Services
{
    public class SendMedicalRecommendation
    {
        public static async Task sendMedicalRecommendation(string note, byte[] pdf, string firstName, string email, string dietetyk)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dietetyk+", "dietetykplus2025@gmail.com"));
            message.To.Add(new MailboxAddress(firstName, email));
            message.Subject = "Zalecenia żywieniowe " + DateTime.Now.ToString("yyyy-MM-dd");

            var body = new TextPart("html")
            {
                Text = $@"
        <p><strong>Przesyłamy Panu/Pani zalecenia żywieniowe z wizyty z dnia {DateTime.Now.ToLongDateString()}</strong></p>
        <p><strong>Treść Zalecenia:</strong> {note}</p>
        <p>Dietetyk: {dietetyk}</p><br><br>
        <p><i>Prosimy pobrać załącznik z Dietą, który znajduje się poniżej</i></p>
    "
            };

            var attachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(new System.IO.MemoryStream(pdf)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "Zalecenia" + DateTime.Now.ToString("yyyy-MM-dd") + ".pdf"
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            message.Body = multipart;

            
            using (var client = new SmtpClient(new ProtocolLogger("smtp.log")))
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("dietetykplus2025@gmail.com", "fgxbaauuftpemenu");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}

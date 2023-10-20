namespace duendeMakeApp.DAO;

using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSenderDAO : IEmailSender
{
    private static EmailSenderDAO? instance;

    public EmailSenderDAO() { }

    public static EmailSenderDAO GetInstance()
    {
        if (instance == null)
        {
            instance = new EmailSenderDAO();
        }
        return instance;
    }

    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("i.am.a.no.reply.email@gmail.com", "aquj mzho vtby abix")
        };

        return client.SendMailAsync(
            new MailMessage(from: "i.am.a.no.reply.email@gmail.com",
                            to: email,
                            subject,
                            message
            ));
    }
}

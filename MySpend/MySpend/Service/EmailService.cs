namespace MySpend.Service;
using System.Net;
using System.Net.Mail;


public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void Send(string to, string subject, string body)
    {
        var smtp = new SmtpClient(
            _config["EmailSettings:SmtpServer"],
            int.Parse(_config["EmailSettings:Port"])
        )
        {
            Credentials = new NetworkCredential(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]
            ),
            EnableSsl = false   
        };

        var message = new MailMessage(
            _config["EmailSettings:From"],
            to,
            subject,
            body
        );

        smtp.Send(message);
    }
}
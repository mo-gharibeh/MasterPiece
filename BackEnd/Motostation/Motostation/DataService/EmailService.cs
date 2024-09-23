using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Motostation.DataService;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendOtpEmail(string email, string otp)
    {
        // Build the email message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("YourAppName", _configuration["Smtp:Username"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Your OTP Code";

        message.Body = new TextPart("plain")
        {
            Text = $"Your OTP code is {otp}"
        };

        // Send the email using SMTP
        using (var client = new SmtpClient())
        {
            try
            {
                // Connect to the SMTP server
                client.Connect(_configuration["Smtp:Host"], Convert.ToInt32(_configuration["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

                // Authenticate with the SMTP server
                client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);

                // Send the message
                client.Send(message);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log if necessary
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            finally
            {
                // Disconnect and dispose the client
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}

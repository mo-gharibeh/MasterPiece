//using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Motostation.DataService;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

     public void SendOtpEmail(string email, string otp)
     {
        MailMessage mail = new MailMessage();
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

        mail.From = new MailAddress("motostation7@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Confirm your email";
        mail.Body = $"Your OTP code is: {otp}";

        smtpServer.Port = 465;
        smtpServer.Credentials = new System.Net.NetworkCredential("motostation7@gmail.com", "yfhsqatniyptcwrz");
        smtpServer.EnableSsl = true;

        try
        {
            smtpServer.Send(mail); // Send the email
            Console.WriteLine("OTP email sent successfully.");
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }

    }


    //public void SendOtpEmail(string email, string otp)
    //{
    //    // Build the email message
    //    var message = new MimeMessage();
    //    message.From.Add(new MailboxAddress("Motostation", _configuration["Smtp:Username"]));
    //    message.To.Add(new MailboxAddress("", email));
    //    message.Subject = "Your OTP Code";

    //    message.Body = new TextPart("plain")
    //    {
    //        Text = $"Your OTP code is {otp}"
    //    };

    //    // Send the email using SMTP
    //    using (var client = new SmtpClient())
    //    {
    //        try
    //        {
    //            // Connect to the SMTP server
    //            client.Connect(_configuration["Smtp:Host"], Convert.ToInt32(_configuration["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

    //            // Authenticate with the SMTP server
    //            client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);

    //            // Send the message
    //            client.Send(message);
    //        }
    //        catch (Exception ex)
    //        {
    //            // Handle exceptions, log if necessary
    //            Console.WriteLine("Error sending email: " + ex.Message);
    //        }
    //        finally
    //        {
    //            // Disconnect and dispose the client
    //            client.Disconnect(true);
    //            client.Dispose();
    //        }
    //    }
    //}
}

namespace Motostation.DataService
{
    public interface IEmailService
    {
        void SendOtpEmail(string email, string otp); // Method declaration in interface should not have a body
    }
}

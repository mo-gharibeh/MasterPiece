namespace Motostation.DTOs
{
    public class OtpConfirmationDto
    {
        public string Email { get; set; }  // The email of the user
        public string Otp { get; set; }    // The OTP entered by the user
    }
}

using System.Text;

namespace Motostation.DTOs
{
    public class PasswordHasherNew
    {
        public static void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var h = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = h.Key;  // key come from method sha512
                passwordHash = h.ComputeHash(Encoding.UTF8.GetBytes(password));  // password ==> hashing 
            }

        }
        public static bool VerifyPasswordHash(string password, byte[] passworddHash, byte[] passwordSalt)
        {
            using (var h = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var newPassHash = h.ComputeHash(Encoding.UTF8.GetBytes(password));  // password ==> hashing
                return newPassHash.SequenceEqual(passworddHash);                                                  // 
            }


        }
    }
}

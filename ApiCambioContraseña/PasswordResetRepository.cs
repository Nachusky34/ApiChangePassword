using System.Security.Cryptography;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;


namespace ApiCambioContraseña
{
    public class PasswordResetRepository
    {

        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            FirebaseClient client = new FirebaseClient("https://dare2go-bfbeb-default-rtdb.firebaseio.com");

            var users = await client.Child("Users").OnceAsync<Usuario>();

            var user = users.FirstOrDefault(x => x.Object.Email.Equals(email));

            if (user is null)
            {
                return false;
            }

            

            await client.Child("Users").Child(user!.Object.Id).PatchAsync(new { password = HashPassword(newPassword) });

            return true;
        }

        public async Task<string> GetEmailFromNumber(string phone)
        {
            FirebaseClient client = new FirebaseClient("https://dare2go-bfbeb-default-rtdb.firebaseio.com");

            var users = await client.Child("Users").OnceAsync<Usuario>();

            var user = users.FirstOrDefault(x => x.Object.Phone.Equals(phone));

            if (user is null)
            {
                return "";
            }

            return user.Object.Email;
        }

        private static string HashPassword(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    sha256.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);
                    byte[] hashBytes = sha256.Hash;

                    StringBuilder hexPass = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        hexPass.AppendFormat("{0:x2}", b);
                    }

                    return hexPass.ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error hashing password", e);
            }
        }

        public class Usuario 
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
        }
    }
}

using System;
using System.Security.Cryptography;

namespace gamestore.Security
{
    public static class PasswordHasher
    {
        // Hash the password using SHA256 (or a stronger algorithm like bcrypt)
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes); // Store as Base64 string
            }
        }

        // Verify the password by comparing it with the stored hashed password
        public static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Hash the entered password using the same method
            var enteredPasswordHash = HashPassword(enteredPassword);

            // Compare the entered password hash with the stored hash
            return enteredPasswordHash == storedPasswordHash;
        }




    }
}

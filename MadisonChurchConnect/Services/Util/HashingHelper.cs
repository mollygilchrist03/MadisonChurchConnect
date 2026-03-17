/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */
using Microsoft.CodeAnalysis.Scripting;

namespace MadisonChurchConnect.Services.Util
{
    /// <summary>
    /// helper class for salting and hashing passwords
    /// </summary>
    public class HashingHelper
    {
        /// <summary>
        /// hash a plain text password using bcrypt
        /// </summary>
        public static string HashPassword(string plainTextPassword)
        {
            // declare variables
            string salt, hashedPassword;

            // generate a salt to ensure unique password hashes
            salt = BCrypt.Net.BCrypt.GenerateSalt();

            // hash the password using the generated salt
            hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword, salt);

            // return the hashed password
            return hashedPassword;
        }

        /// <summary>
        /// verify a plain text password against a hashed password
        /// </summary>
        public static bool VerifyPassword(string hashedPassword, string plainTextPassword)
        {
            // verify the plain text password against the hashed password and return the result
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}
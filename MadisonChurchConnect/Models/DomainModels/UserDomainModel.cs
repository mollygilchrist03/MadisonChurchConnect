/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Models.DomainModels
{
    public class UserDomainModel
    {
        // class level properties
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// default constructor for user domain model
        /// </summary>
        public UserDomainModel()
        {
            Id = 0;
            FirstName = "";
            LastName = "";
            Username = "";
            PasswordHash = "";
            Email = "";
            PhoneNumber = null;
        }
    }
}

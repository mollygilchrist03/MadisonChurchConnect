/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

using System.ComponentModel.DataAnnotations;

namespace MadisonChurchConnect.Models.ViewModels
{
    public class UserViewModel
    {
        // class level properties
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// default constructor for user view model
        /// </summary>
        public UserViewModel()
        {
            Id = 0;
            FirstName = "";
            LastName = "";
            Email = "";
            Username = "";
            PasswordHash = "";
            PhoneNumber = null;
        }
    }
}

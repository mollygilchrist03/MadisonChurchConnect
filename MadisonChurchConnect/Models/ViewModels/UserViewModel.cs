/*
 * Molly Gilchrist
 * 2/5/2026
 * STG-456
 * Capstone Project
 */

using System.ComponentModel.DataAnnotations;

namespace MadisonChurchConnect.Models.ViewModels
{
    public class UserViewModel
    {
        // class level properties
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public UserViewModel()
        {
            // default values for properties
            UserId = 0;
            FirstName = "";
            LastName = "";
            Username = "";
            PasswordHash = "";
            Email = "";
            PhoneNumber = "";
        }
    }
}

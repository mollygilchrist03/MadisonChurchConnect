/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */
using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Models.ViewModels;
namespace MadisonChurchConnect.Services.Mapper
{
    public class UserMapper
    {
        /// <summary>
        /// map a user view model to a user domain model
        /// </summary>
        public static UserDomainModel ToDomainModel(UserViewModel viewModel)
        {
            // declare domain model
            UserDomainModel domainModel;

            // throw null exception if view model is null
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            // map the view model properties to the domain model
            domainModel = new UserDomainModel
            {
                Id = viewModel.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Username = viewModel.Username,
                PasswordHash = viewModel.PasswordHash,
                Email = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber
            };

            // return the domain model
            return domainModel;
        }

        /// <summary>
        /// map a user domain model to a user view model
        /// </summary>
        public static UserViewModel FromDomainModel(UserDomainModel domainModel)
        {
            // declare and initialize the view model
            UserViewModel viewModel = new UserViewModel
            {
                // map all properties from the domain model
                Id = domainModel.Id,
                FirstName = domainModel.FirstName,
                LastName = domainModel.LastName,
                Username = domainModel.Username,
                PasswordHash = domainModel.PasswordHash,
                Email = domainModel.Email,
                PhoneNumber = domainModel.PhoneNumber
            };

            // return the view model
            return viewModel;
        }
    }
}
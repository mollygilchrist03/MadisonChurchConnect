/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

using System.Diagnostics;
using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.Interfaces;
using MadisonChurchConnect.Services.Mapper;
using MadisonChurchConnect.Services.Util;

namespace MadisonChurchConnect.Services.BusinessLogic
{
    public class UserLogic
    {
        // class level variables
        private IUserDAO _userDAO;
        private readonly ILogger<UserLogic> _logger;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public UserLogic(IUserDAO userDAO, ILogger<UserLogic> logger)
        {
            _userDAO = userDAO;
            _logger = logger;
        }

        /// <summary>
        /// add a new user from a user view model
        /// </summary>
        public int AddUser(UserViewModel viewUser)
        {
            // declare domain user
            UserDomainModel domainUser;

            try
            {
                // map the view model to a domain model
                domainUser = UserMapper.ToDomainModel(viewUser);
            }
            catch (ArgumentNullException)
            {
                // return -1 to indicate the parameter was null
                return -1;
            }

            // hash the user's password before saving
            domainUser.PasswordHash = HashingHelper.HashPassword(domainUser.PasswordHash);

            // send the domain model to the dao and return the result
            return _userDAO.AddUser(domainUser);
        }

        /// <summary>
        /// validate a user's credentials by username and password
        /// </summary>
        public (bool isValidated, UserViewModel? viewUser) ValidateUserCredentials(string username, string password)
        {
            // declare variables
            UserDomainModel? domainUser;
            UserViewModel? viewUser = null;
            bool userExists = false, isValidated = false;
            Stopwatch lookupStopwatch = Stopwatch.StartNew();

            // look up the user by username
            (userExists, domainUser) = _userDAO.GetUserFromUsername(username);
            lookupStopwatch.Stop();

            _logger.LogInformation(
                "User lookup finished for username '{Username}' in {ElapsedMilliseconds} ms. Found user: {UserExists}",
                username,
                lookupStopwatch.ElapsedMilliseconds,
                userExists);

            // verify the password if the user was found
            if (userExists && domainUser != null)
            {
                Stopwatch passwordVerificationStopwatch = Stopwatch.StartNew();
                bool isPasswordValid = HashingHelper.VerifyPassword(domainUser.PasswordHash, password);
                passwordVerificationStopwatch.Stop();

                _logger.LogInformation(
                    "Password verification finished for username '{Username}' in {ElapsedMilliseconds} ms. Success: {IsPasswordValid}",
                    username,
                    passwordVerificationStopwatch.ElapsedMilliseconds,
                    isPasswordValid);

                if (isPasswordValid)
                {
                    // map the domain model to a view model
                    viewUser = UserMapper.FromDomainModel(domainUser);

                    // set isvalidated to true
                    isValidated = true;
                }
            }
            else
            {
                _logger.LogInformation(
                    "Skipping password verification for username '{Username}' because no matching user was found.",
                    username);
            }

            // return whether the user was validated and the view model
            return (isValidated, viewUser);
        }

        /// <summary>
        /// get a user view model by their id — used to verify note ownership
        /// </summary>
        public (bool wasUserFound, UserViewModel? viewUser) GetUserFromId(int userId)
        {
            // declare variables
            UserDomainModel? domainUser;
            UserViewModel? viewUser = null;
            bool userExists = false;

            // look up the user by id
            (userExists, domainUser) = _userDAO.GetUserFromId(userId);

            // map to view model if the user was found
            if (userExists && domainUser != null)
            {
                viewUser = UserMapper.FromDomainModel(domainUser);
            }

            // return whether the user was found and the view model
            return (userExists, viewUser);
        }
    }
}

/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;

namespace MadisonChurchConnect.Services.Interfaces
{
    public interface IUserDAO
    {
        /// <summary>
        /// add a new user to the database and return the new user's id
        /// </summary>
        int AddUser(UserDomainModel user);

        /// <summary>
        /// get a user based on their username
        /// </summary>
        (bool wasUserFound, UserDomainModel? foundUser) GetUserFromUsername(string username);

        /// <summary>
        /// get a user based on their id — used to verify note ownership
        /// </summary>
        (bool wasUserFound, UserDomainModel? foundUser) GetUserFromId(int userId);
    }
}
/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace MadisonChurchConnect.Services.DataAccess
{
    public class UserDAO : IUserDAO
    {
        // class level variables
        private readonly string _connectionString = "";

        private string _query = "";

        public UserDAO(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        ///<inheritdoc/>
        public int AddUser(UserDomainModel user)
        {
            // declare variables
            SqlTransaction transaction;
            int newId;

            // create a new sql connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // begin a sql transaction
                transaction = connection.BeginTransaction();

                // create a query to insert a new user
                _query = "INSERT INTO Users (FirstName, LastName, Username, PasswordHash, Email, PhoneNumber) " +
                         "OUTPUT INSERTED.UserId " +
                         "VALUES (@FirstName, @LastName, @Username, @PasswordHash, @Email, @PhoneNumber)";

                // create a sql command with the query and transaction
                using (SqlCommand cmd = new SqlCommand(_query, connection, transaction))
                {
                    // add parameters to the command
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@Email", user.Email);

                    // use dbnull if phone number is null
                    cmd.Parameters.AddWithValue("@PhoneNumber", (object?)user.PhoneNumber ?? DBNull.Value);

                    try
                    {
                        // execute the query and capture the new user's id
                        newId = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        // rollback the transaction if the query fails
                        transaction.Rollback();

                        // return -1 to indicate failure
                        return -1;
                    }
                }

                // commit the transaction if successful
                transaction.Commit();
            }

            // return the new user's id
            return newId;
        }

        ///<inheritdoc/>
        public (bool wasUserFound, UserDomainModel? foundUser) GetUserFromUsername(string username)
        {
            // declare and initialize found user
            UserDomainModel? foundUser = null;

            // create a new sql connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // create a query to get a user by username
                _query = @"SELECT UserId, FirstName, LastName, Username, PasswordHash, Email, PhoneNumber
                           FROM Users
                           WHERE Username = @Username";

                // create a sql command with the query
                using (SqlCommand cmd = new SqlCommand(_query, connection))
                {
                    // add the username parameter
                    cmd.Parameters.AddWithValue("@Username", username);

                    // execute the command and read the results
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // loop through the reader
                        while (reader.Read())
                        {
                            // only map the first result
                            if (foundUser == null)
                            {
                                // map the reader columns to the domain model
                                foundUser = new UserDomainModel
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),

                                    // map phone number to null if it is dbnull
                                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                };
                            }
                        }
                    }
                }
            }

            // return whether the user was found and the found user
            return (foundUser != null, foundUser);
        }

        ///<inheritdoc/>
        public (bool wasUserFound, UserDomainModel? foundUser) GetUserFromId(int userId)
        {
            // declare and initialize found user
            UserDomainModel? foundUser = null;

            // create a new sql connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // create a query to get a user by id
                _query = @"SELECT UserId, FirstName, LastName, Username, PasswordHash, Email, PhoneNumber
                           FROM Users
                           WHERE UserId = @UserId";

                // create a sql command with the query
                using (SqlCommand cmd = new SqlCommand(_query, connection))
                {
                    // add the userid parameter
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    // execute the command and read the results
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // loop through the reader
                        while (reader.Read())
                        {
                            // only map the first result
                            if (foundUser == null)
                            {
                                // map the reader columns to the domain model
                                foundUser = new UserDomainModel
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),

                                    // map phone number to null if it is dbnull
                                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                };
                            }
                        }
                    }
                }
            }

            // return whether the user was found and the found user
            return (foundUser != null, foundUser);
        }
    }
}
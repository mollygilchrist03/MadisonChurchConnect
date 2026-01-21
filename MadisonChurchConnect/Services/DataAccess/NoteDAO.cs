/*
 * Molly Gilchrist
 * 1/21/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace MadisonChurchConnect.Services.DataAccess
{
    public class NoteDAO : INoteDAO
    {
        // class level variables
        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MadisonChurchConnectDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        private string _query = "";

        /// <inheritdoc/>
        public int AddNewNote(NoteDomainModel newNote)
        {
            // declare variables
            SqlTransaction transaction;
            int newNoteId;

            // create a new sqlconnection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // begin sql transaction
                transaction = connection.BeginTransaction();

                // create a query for adding a new note
                _query = "INSERT INTO Notes (NoteId, NoteTitle, NoteDate, NoteContent) " +
                         "OUTPUT INSERTED.Id " +
                         "VALUES (@NoteId, @NoteTitle, @NoteDate, @NoteContent)";

                // create a sqlcommand to send statements to the mssql server database
                using (SqlCommand cmd = new SqlCommand(_query, connection, transaction))
                {
                    // add the parameters for the cmd
                    cmd.Parameters.AddWithValue("@NoteId", newNote.NoteId);
                    cmd.Parameters.AddWithValue("@NoteTitle", newNote.NoteTitle);
                    cmd.Parameters.AddWithValue("@NoteDate", newNote.NoteDate);
                    cmd.Parameters.AddWithValue("@NoteContent", newNote.NoteContent);

                    // try/catch for the execute statement
                    try
                    {
                        // execute query and capture resulting int inot newNoteId
                        newNoteId = (int)cmd.ExecuteScalar();
                    } 
                    catch (Exception)
                    {
                        // rollback transaction if query fails
                        transaction.Rollback();

                        // return -1 to show exception
                        return -1;
                    }
                }
                // commit the transaction if successful
                transaction.Commit();
            }
            // return the new note id
            return newNoteId;
        }

        /// <inheritdoc/>
        public int DeleteNote(int noteId)
        {
            // delcare and init
            int rowsAffected = 0;

            // create a new sql connection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // create the query
                _query = "DELETE FROM Notes WHERE NoteId = @NoteId";

                // cretae a sql command to send the statement to the database
                using (SqlCommand cmd = new SqlCommand(_query, connection))
                {
                    // add the paramter for the command
                    cmd.Parameters.AddWithValue("@NoteId", noteId);

                    // try/catch for execute statement
                    try
                    {
                        // execute non-query and put it in the rowsAffected
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        // return -1 to show exception
                        return -1;
                    }
                }
            }
            // return rows affected
            return rowsAffected;
        }

        /// <inheritdoc/>
        public int EditNote(NoteDomainModel updatedNote)
        {
            // declare and int
            int rowsAffected = 0;
            SqlTransaction transaction;

            // create a new sqlconnection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // begin sql transaction
                transaction = connection.BeginTransaction();

                // sql query to update the note 
                _query = @"UPDATE Notes 
                           SET NoteTitle = @NoteTitle, 
                               NoteDate = @NoteDate,
                               NoteContent = @NoteContent
                           WHERE NoteId = @NoteId";

                // create a sqlcommand to send statements to the mssql server database
                using (SqlCommand cmd = new SqlCommand(_query, connection))
                {
                    // add the parameters for the cmd
                    cmd.Parameters.AddWithValue("@NoteId", updatedNote.NoteId);
                    cmd.Parameters.AddWithValue("@NoteTitle", updatedNote.NoteTitle);
                    cmd.Parameters.AddWithValue("@NoteDate", updatedNote.NoteDate);
                    cmd.Parameters.AddWithValue("@NoteContent", updatedNote.NoteContent);

                    // try/catch for the execute statement
                    try
                    {
                        // execute the update query
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        // rollback transaction if query fails
                        transaction.Rollback();

                        // return -1 to show exception
                        return -1;
                    }
                }
                // commit the transaction if successful
                transaction.Commit();
            }
            // return the number of rows affected
            return rowsAffected;
        }

        /// <inheritdoc/>
        public List<NoteDomainModel> GetAllNotes()
        {
            // create a list to store all retrieved notes
            List<NoteDomainModel> notes = new List<NoteDomainModel>();

            // create a new sqlconnection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // sql query to select all notes from the notes table
                _query = "SELECT NoteId, NoteTitle, NoteDate, NoteContent " +
                        "FROM Notes " +
                        "ORDER BY NoteDate DESC";

                // create a sqlcommand to send statements to the mssql server database
                using (SqlCommand cmd = new SqlCommand(_query, connection))
                {
                    // execute the query and get a reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // loop through each row in the result set
                        while (reader.Read())
                        {
                            // create a new note object and populate it with data
                            NoteDomainModel note = new NoteDomainModel
                            {
                                NoteId = reader.GetInt32(0),
                                NoteTitle = reader.GetString(1),
                                NoteDate = reader.GetDateTime(2),
                                NoteContent = reader.GetString(3)
                            };

                            // add the note to the list
                            notes.Add(note);
                        }
                    }
                }
            }
            return notes;
        }

        /// <inheritdoc/>
        public NoteDomainModel? GetNoteById(int noteId)
        {
            // declare and init
            NoteDomainModel? note = null;

            // create a new sql connection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // open the connection
                connection.Open();

                // create a query for selecting the note id
                _query = "SELECT NoteId, NoteTitle, NoteDate, NoteContent " +
                         "FROM Notes " +
                         "WHERE NoteId = @NoteId";

                // create a sql command to send the query to the database
                using (SqlCommand cmd = new SqlCommand(_query, connection))
                {
                    // add the paramter for the command
                    cmd.Parameters.AddWithValue("@NoteId", noteId);

                    // set up the reader and execute the command
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // loop through the reader
                        while (reader.Read())
                        {
                            note = new NoteDomainModel
                            {
                                NoteId = reader.GetInt32(0),
                                NoteTitle = reader.GetString(1),
                                NoteDate = reader.GetDateTime(2),
                                NoteContent = reader.GetString(3)
                            };
                        }
                    }
                }
            }
            // return the note 
            return note;
        }
    }
}

/*
 * Molly Gilchrist
 * 1/19/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;

namespace MadisonChurchConnect.Services.Interfaces
{
    public interface INoteDAO
    {
        /// <summary>
        /// adds a new note to the database
        /// </summary>
        /// <param name="newNote"></param>
        /// <returns></returns>
        int AddNewNote(NoteDomainModel newNote);

        /// <summary>
        /// retrieves a specific note by its id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        NoteDomainModel GetNoteById(int noteId);

        /// <summary>
        /// gets all saved notes from the database
        /// </summary>
        /// <returns></returns>
        List<NoteDomainModel> GetAllNotes();

        /// <summary>
        /// deletes a note by id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        int DeleteNote(int noteId);

        /// <summary>
        /// allows users to edit saved notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        int EditNote(int noteId);
    }
}

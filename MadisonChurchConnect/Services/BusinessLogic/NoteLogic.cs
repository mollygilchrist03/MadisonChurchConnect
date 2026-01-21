/*
 * Molly Gilchrist
 * 1/19/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.Interfaces;

namespace MadisonChurchConnect.Services.BusinessLogic
{
    public class NoteLogic
    {
        // class level variables
        private INoteDAO _noteDAO;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="noteDAO"></param>
        public NoteLogic(INoteDAO noteDAO)
        {
            _noteDAO = noteDAO;
        }

        /// <summary>
        /// adds a new note from a note view model
        /// </summary>
        /// <param name="newNote"></param>
        /// <returns></returns>
        public int AddNewNote(NoteViewModel newViewNote)
        {
            
        }

        /// <summary>
        /// retrieves a specific note by its id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public NoteViewModel GetNoteById(int noteId)
        {

        }

        /// <summary>
        /// gets all saved notes from the database
        /// </summary>
        /// <returns></returns>
        public List<NoteViewModel> GetAllNotes()
        {

        }

        /// <summary>
        /// deletes a note by id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public int DeleteNote(int noteId)
        {

        }

        /// <summary>
        /// allows users to edit saved notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public int EditNote(NoteViewModel updatedViewNote)
        {

        }
    }
}

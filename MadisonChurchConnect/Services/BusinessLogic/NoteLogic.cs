/*
 * Molly Gilchrist
 * 1/19/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.Interfaces;
using MadisonChurchConnect.Services.Mapper;

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
            // declare
            NoteDomainModel newDomainNote;

            // try/catch for mapping the viewNote to domainNote
            try
            {
                // use the mapper to map the view model to a domain model
                newDomainNote = NoteMapper.ToDomainModel(newViewNote);
            }
            catch (ArgumentNullException)
            {
                // return to show that the parameter was null
                return -1;
            }

            // send the domain model to the dao and return result
            return _noteDAO.AddNewNote(newDomainNote);
        }

        /// <summary>
        /// retrieves a specific note by its id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public NoteViewModel GetNoteById(int noteId)
        {
            // declare a new domain note and get note id
            NoteDomainModel domainNote = _noteDAO.GetNoteById(noteId);

            // if domain note is null, return null
            if (domainNote == null)
            {
                return null;
            }

            // map the domain note to a view note and retrun
            return NoteMapper.ToViewModel(domainNote);
        }

        /// <summary>
        /// gets all saved notes
        /// </summary>
        /// <returns></returns>
        public List<NoteViewModel> GetAllNotes()
        {
            // get all games from dao
            List<NoteDomainModel> domainNotes = _noteDAO.GetAllNotes();

            // map to view models
            List<NoteViewModel> viewNotes = new List<NoteViewModel>();

            // loop through the list of notes
            foreach (NoteDomainModel domainNote in domainNotes)
            {
                // map each note to a view note
                viewNotes.Add(NoteMapper.ToViewModel(domainNote));
            }

            // return the list of view notes
            return viewNotes;
        }

        /// <summary>
        /// deletes a note by id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNote(int noteId)
        {
            // get the result of the deleted game
            int result = _noteDAO.DeleteNote(noteId);

            // return the result and make sure it's more than 0
            return result > 0;
        }

        /// <summary>
        /// allows users to edit saved notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public int EditNote(NoteViewModel updatedViewNote)
        {
            // declare
            NoteDomainModel updatedDomainNote;

            // try/catch for mapping the viewNote to domainNote
            try
            {
                // use the mapper to map the view model to a domain model
                updatedDomainNote = NoteMapper.ToDomainModel(updatedViewNote);
            }
            catch (ArgumentNullException)
            {
                // return -1 to show that the parameter was null
                return -1;
            }

            // send the domain model to the dao and return result
            return _noteDAO.EditNote(updatedDomainNote);
        }
    }
}

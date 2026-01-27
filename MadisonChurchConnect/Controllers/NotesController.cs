/*
 * Molly Gilchrist
 * 1/15/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class NotesController : Controller
    {
        // class level variables
        private readonly NoteLogic _noteLogic;

        // <summary>
        /// parameterized constructor for dependency injection
        /// </summary>
        /// <param name="noteLogic"></param>
        public NotesController(NoteLogic noteLogic)
        {
            _noteLogic = noteLogic;
        }

        /// <summary>
        /// returns the main notes page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // get all notes from business logic
            List<NoteViewModel> notes = _noteLogic.GetAllNotes();

            return View(notes);
        }

        /// <summary>
        /// displays a specific note
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            // get the note by its id from business logic
            NoteViewModel note = _noteLogic.GetNoteById(id);

            // check if note is null
            if (note == null)
            {
                return NotFound();
            }

            // return the view with the note
            return View(note);
        }

        /// <summary>
        /// displays the create note form
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// handles note form submission
        /// </summary>
        /// <param name="newNote"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(NoteViewModel newNote)
        {
            // check if the model state is valid
            if (ModelState.IsValid)
            {
                // add the new note using business logic
                int result = _noteLogic.AddNewNote(newNote);

                // check if the add was successful
                if (result > 0)
                {
                    // redirect to index on success
                    return RedirectToAction("Index");
                }
                else
                {
                    // add error message if failed
                    ModelState.AddModelError("", "Failed to create note. Please try again.");
                }
            }

            // return the view with the model if success
            return View(newNote);
        }

        /// <summary>
        /// display the edit form
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            // get the note by id from business logic
            NoteViewModel note = _noteLogic.GetNoteById(id);

            // check if note is null
            if (note == null)
            {
                return NotFound();
            }

            // return the view with the note
            return View(note);
        }

        /// <summary>
        /// handles the edit form submission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedNote"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(int id, NoteViewModel updatedNote)
        {
            // check if the id matches
            if (id != updatedNote.NoteId)
            {
                return NotFound();
            }

            // check if model state is valid
            if (ModelState.IsValid)
            {
                // update the note using business logic
                int result = _noteLogic.EditNote(updatedNote);

                // check if the update was successful
                if (result > 0)
                {
                    // redirect to index on success
                    return RedirectToAction("Index");
                }
                else
                {
                    // add error message if failed
                    ModelState.AddModelError("", "Failed to update note. Please try again.");
                }
            }

            // return the view with the model if validation fails
            return View(updatedNote);
        }

        /// <summary>
        /// displays the delete confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            // get the note by id from business logic
            NoteViewModel note = _noteLogic.GetNoteById(id);

            // check if note is null
            if (note == null)
            {
                return NotFound();
            }

            // return the view with the note
            return View(note);
        }

        /// <summary>
        /// handles delete confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // delete the note using business logic
            bool result = _noteLogic.DeleteNote(id);

            // check if the delete was successful
            if (!result)
            {
                // add error message if failed
                TempData["ErrorMessage"] = "Failed to delete note. Please try again.";
            }

            // redirect to index
            return RedirectToAction("Index");
        }
    }
}

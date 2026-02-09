/*
 * Molly Gilchrist
 * 1/19/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Models.ViewModels;

namespace MadisonChurchConnect.Services.Mapper
{
    public class NoteMapper
    {
        /// <summary>
        /// maps a NoteViewModel to a NoteDomainModel, throwing an ArgumentNullException if the viewNote is null
        /// </summary>
        /// <param name="viewNote"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static NoteDomainModel ToDomainModel(NoteViewModel viewNote)
        {
            // declare and init
            NoteDomainModel domainModel;

            // check for null viewModel
            if (viewNote == null)
            {
                // throw null exception if null
                throw new ArgumentNullException(nameof(viewNote));
            }

            // create a user domain model based on the viewModel
            domainModel = new NoteDomainModel
            {
                NoteId = viewNote.NoteId,
                NoteTitle = viewNote.NoteTitle,
                NoteDate = viewNote.NoteDate,
                NoteContent = viewNote.NoteContent,
            };

            // Return domain model
            return domainModel;
        }

        /// <summary>
        /// maps a NoteDomainModel to a NoteViewModel, creating a preview of the note content that is 150 characters long or less, adding "..." if the content is longer than 150 characters
        /// </summary>
        /// <param name="domainNote"></param>
        /// <returns></returns>
        public static NoteViewModel ToViewModel(NoteDomainModel domainNote)
        {
            // declare and init
            NoteViewModel viewNote = new NoteViewModel
            {
                // map the properties of the note
                NoteId = domainNote.NoteId,
                NoteTitle = domainNote.NoteTitle,
                NoteDate = domainNote.NoteDate,
                NoteContent = domainNote.NoteContent,
                NotePreview = domainNote.NoteContent.Length > 150 ? domainNote.NoteContent.Substring(0, 150) + "..." : domainNote.NoteContent
            };

            // return the mapped view note
            return viewNote;
        }
    }
}

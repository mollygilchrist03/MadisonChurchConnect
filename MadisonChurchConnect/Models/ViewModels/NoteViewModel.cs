/*
 * Molly Gilchrist
 * 1/19/2026
 * STG-456
 * Capstone Project
 */

using System.ComponentModel.DataAnnotations;

namespace MadisonChurchConnect.Models.ViewModels
{
    public class NoteViewModel
    {
        // class level properties
        public int NoteId { get; set; }

        [Required]
        public string NoteTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTime NoteDate { get; set; }

        [Required]
        public string NoteContent { get; set; }
        public string NotePreview { get; set; }

        /// <summary>
        /// default constructor for ntoe view model
        /// </summary>
        public NoteViewModel()
        {
            NoteTitle = string.Empty;
            NoteDate = DateTime.Now;
            NoteContent = string.Empty;
            NotePreview = string.Empty;
        }
    }
}

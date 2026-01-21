/*
 * Molly Gilchrist
 * 1/19/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Models.DomainModels
{
    public class NoteDomainModel
    {
        // class level properties
        public int NoteId { get; set; }
        public string NoteTitle { get; set; }
        public DateTime NoteDate { get; set; }
        public string NoteContent { get; set; }

        /// <summary>
        /// default constructor for note domain model
        /// </summary>
        public NoteDomainModel()
        {
            NoteId = 0;
            NoteTitle = string.Empty;
            NoteDate = DateTime.Now;
            NoteContent = string.Empty;
        }
    }
}

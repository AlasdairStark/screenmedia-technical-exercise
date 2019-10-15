using System.ComponentModel.DataAnnotations;

namespace Screenmedia.ToDo.Web.Models.ToDoNotes
{
    public class ToDoNoteViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = default!;
        
        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public bool Done { get; set; } = false;
    }
}

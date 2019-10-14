using System;

namespace Screenmedia.ToDo.Web.Models.ToDoNotes
{
    public class ToDoNoteViewModel
    {
        public int Id { get; set; }
        // TODO - Re-evaluate use of the null-forgiving operator
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public bool Done { get; set; } = false;
        public bool Deleted { get; set; } = false;
    }
}

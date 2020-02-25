﻿using System.Collections.Generic;

namespace Screenmedia.ToDo.Web.Models.ToDoNotes
{
    public class ToDoNotesViewModel
    {
        public IList<ToDoNoteViewModel> ToDoNotes { get; } = new List<ToDoNoteViewModel>();

        public int PageCount { get; set; }

        public int PageNumber { get; set; }
    }
}

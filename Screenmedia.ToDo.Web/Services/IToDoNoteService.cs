using Screenmedia.ToDo.Web.Models.ToDoNotes;

namespace Screenmedia.ToDo.Web.Services
{
    public interface IToDoNoteService
    {
        ToDoNotesViewModel List();

        void Create(ToDoNoteViewModel toDoNoteViewModel, string applicationUserId);

        ToDoNoteViewModel Read(int id);

        void Update(ToDoNoteViewModel toDoNoteViewModel);

        void Delete(int id);
    }
}

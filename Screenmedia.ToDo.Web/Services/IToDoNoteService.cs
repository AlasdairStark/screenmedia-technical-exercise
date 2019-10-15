using Screenmedia.ToDo.Web.Models.ToDoNotes;

namespace Screenmedia.ToDo.Web.Services
{
    public interface IToDoNoteService
    {
        void Create(ToDoNoteViewModel toDoNoteViewModel, string applicationUserId);

        ToDoNoteViewModel Read(int id, string applicationUserId);

        ToDoNotesViewModel List(string applicationUserId);

        void Update(ToDoNoteViewModel toDoNoteViewModel, string applicationUserId);

        void Delete(int id, string applicationUserId);
    }
}

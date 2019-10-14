using AutoMapper;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Models.ToDoNotes;

namespace Screenmedia.ToDo.Web.AutoMapper
{
    public class ToDoNotesProfile : Profile
    {
        public ToDoNotesProfile()
        {
            CreateMap<ToDoNote, ToDoNoteViewModel>();
        }
    }
}

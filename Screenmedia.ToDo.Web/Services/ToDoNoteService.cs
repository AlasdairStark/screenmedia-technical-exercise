using System;
using System.Linq;
using Screenmedia.ToDo.Web.Data;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Exceptions;
using Screenmedia.ToDo.Web.Models.ToDoNotes;

namespace Screenmedia.ToDo.Web.Services
{
    public class ToDoNoteService : IToDoNoteService
    {
        private const int MaxListSize = 10;

        private readonly IApplicationDbContext _applicationDbContext;

        public ToDoNoteService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Create(ToDoNoteViewModel toDoNoteViewModel, string applicationUserId)
        {
            if (toDoNoteViewModel == null)
                throw new ArgumentNullException(nameof(toDoNoteViewModel));

            var toDoNote = new ToDoNote
            {
                Title = toDoNoteViewModel.Title,
                Description = toDoNoteViewModel.Description,
                ApplicationUserId = applicationUserId
            };

            _applicationDbContext.ToDoNotes.Add(toDoNote);
            _applicationDbContext.SaveChanges();
        }

        public ToDoNoteViewModel Read(int id, string applicationUserId)
        {
            var toDoNote = _applicationDbContext.ToDoNotes
                .FirstOrDefault(n => n.Id == id && 
                                     n.ApplicationUserId == applicationUserId &&
                                     !n.Deleted);

            if (toDoNote == null)
                throw new EntityNotFoundException<ToDoNote>(id);

            return new ToDoNoteViewModel
            {
                Id = toDoNote.Id,
                Title = toDoNote.Title,
                Description = toDoNote.Description,
                Done = toDoNote.Done
            };
        }

        public ToDoNotesViewModel List(string applicationUserId)
        {
            var toDoNoteViewModels = _applicationDbContext.ToDoNotes
                .Where(n => n.ApplicationUserId == applicationUserId && 
                            !n.Deleted)
                .Select(n => new ToDoNoteViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    Done = n.Done
                })
                .Take(MaxListSize)
                .ToList();

            var viewModel = new ToDoNotesViewModel();

            foreach(var toDoNoteViewModel in toDoNoteViewModels)
            {
                viewModel.ToDoNotes.Add(toDoNoteViewModel);
            }

            return viewModel;
        }

        public void Update(ToDoNoteViewModel toDoNoteViewModel, string applicationUserId)
        {
            if (toDoNoteViewModel == null)
                throw new ArgumentNullException(nameof(toDoNoteViewModel));

            var toDoNote = _applicationDbContext.ToDoNotes
                .FirstOrDefault(n => n.Id == toDoNoteViewModel.Id && 
                                     n.ApplicationUserId == applicationUserId &&
                                     !n.Deleted);

            if (toDoNote == null)
                throw new EntityNotFoundException<ToDoNote>(toDoNoteViewModel.Id);

            toDoNote.Title = toDoNoteViewModel.Title;
            toDoNote.Description = toDoNoteViewModel.Description;
            toDoNote.Done = toDoNoteViewModel.Done;

            _applicationDbContext.SaveChanges();
        }

        public void Delete(int id, string applicationUserId)
        {
            var toDoNote = _applicationDbContext.ToDoNotes
                .FirstOrDefault(n => n.Id == id && 
                                     n.ApplicationUserId == applicationUserId &&
                                     !n.Deleted);

            if (toDoNote == null)
                throw new EntityNotFoundException<ToDoNote>(id);

            toDoNote.Deleted = true;

            _applicationDbContext.SaveChanges();
        }
    }
}

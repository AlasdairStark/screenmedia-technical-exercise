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
        private const int PageSize = 5;

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

        public ToDoNotesViewModel List(string applicationUserId, int page)
        {
            var toDoNoteViewModelsQuery = _applicationDbContext.ToDoNotes
                .Where(n => n.ApplicationUserId == applicationUserId &&
                            !n.Deleted);

            var toDoNoteCount = toDoNoteViewModelsQuery.Count();

            if (toDoNoteCount == 0)
                return new ToDoNotesViewModel();

            var pageCount = (int)Math.Ceiling((double)toDoNoteViewModelsQuery.Count() / PageSize);

            if (page < 1 || page > pageCount)
                throw new ArgumentOutOfRangeException(nameof(page));

            var toDoNoteViewModels = toDoNoteViewModelsQuery
                .Select(n => new ToDoNoteViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    Done = n.Done
                })
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new ToDoNotesViewModel();

            foreach(var toDoNoteViewModel in toDoNoteViewModels)
            {
                viewModel.ToDoNotes.Add(toDoNoteViewModel);
            }

            viewModel.PageCount = pageCount;
            viewModel.PageNumber = page;

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

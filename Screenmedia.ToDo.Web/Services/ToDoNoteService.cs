﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Screenmedia.ToDo.Web.Data;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Models.ToDoNotes;

namespace Screenmedia.ToDo.Web.Services
{
    public class ToDoNoteService : IToDoNoteService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public ToDoNoteService(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public ToDoNotesViewModel List()
        {
            var toDoNoteViewModels = _applicationDbContext.ToDoNotes
                .Select(n => _mapper.Map<ToDoNoteViewModel>(n))
                .ToList();

            var viewModel = new ToDoNotesViewModel();

            foreach(var toDoNoteViewModel in toDoNoteViewModels)
            {
                viewModel.ToDoNotes.Add(toDoNoteViewModel);
            }

            return viewModel;
        }

        public void Create(ToDoNoteViewModel toDoNoteViewModel, string applicationUserId)
        {
            var toDoNote = _mapper.Map<ToDoNote>(toDoNoteViewModel);
            toDoNote.ApplicationUserId = applicationUserId;

            _applicationDbContext.ToDoNotes.Add(toDoNote);
            _applicationDbContext.SaveChanges();
        }

        public ToDoNoteViewModel Read(int id)
        {
            var toDoNote = _applicationDbContext.ToDoNotes
                .FirstOrDefault(n => n.Id == id);

            // TODO - Refactor
            if (toDoNote == null)
                throw new Exception();

            return _mapper.Map<ToDoNoteViewModel>(toDoNote);
        }

        public void Update(ToDoNoteViewModel toDoNoteViewModel)
        {
            if (toDoNoteViewModel == null)
                throw new ArgumentNullException(nameof(toDoNoteViewModel));

            var toDoNote = _applicationDbContext.ToDoNotes
                .FirstOrDefault(n => n.Id == toDoNoteViewModel.Id);

            // TODO - Refactor
            if (toDoNote == null)
                throw new Exception();

            toDoNote.Title = toDoNoteViewModel.Title;
            toDoNote.Description = toDoNoteViewModel.Description;

            _applicationDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var toDoNote = _applicationDbContext.ToDoNotes
                .FirstOrDefault(n => n.Id == id);

            // TODO - Refactor
            if (toDoNote == null)
                throw new Exception();

            toDoNote.Deleted = true;

            _applicationDbContext.SaveChanges();
        }
    }
}

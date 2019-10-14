using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Screenmedia.ToDo.Web.Data;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Models.ToDoNotes;
using Screenmedia.ToDo.Web.Services;

namespace Screenmedia.ToDo.Web.Controllers
{
    [Authorize]
    public class ToDoNotesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IToDoNoteService _toDoNoteService;

        public ToDoNotesController(
            UserManager<IdentityUser> userManager,
            IToDoNoteService toDoNoteService)
        {
            _userManager = userManager;
            _toDoNoteService = toDoNoteService;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            // TODO - Handling of async method
            //var userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            var viewModel = _toDoNoteService.Read(id);

            return View("_ToDoNote", viewModel);
        }
    }
}

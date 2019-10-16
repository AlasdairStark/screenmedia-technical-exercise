using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Exceptions;
using Screenmedia.ToDo.Web.Extensions;
using Screenmedia.ToDo.Web.Models.ToDoNotes;
using Screenmedia.ToDo.Web.Services;

namespace Screenmedia.ToDo.Web.Controllers
{
    [Authorize]
    public class ToDoNotesController : Controller
    {
        private readonly ILogger<ToDoNotesController> _logger;
        private readonly IToDoNoteService _toDoNoteService;

        public ToDoNotesController(
            IToDoNoteService toDoNoteService,
            ILogger<ToDoNotesController> logger)
        {
            _toDoNoteService = toDoNoteService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult List()
        {
            var viewModel = _toDoNoteService.List(User.GetId());

            return View("List", viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDoNoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            _toDoNoteService.Create(viewModel, User.GetId());

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var viewModel = _toDoNoteService.Read(id, User.GetId());
                return View("Edit", viewModel);
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ToDoNoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            try
            {
                _toDoNoteService.Update(viewModel, User.GetId());
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _toDoNoteService.Delete(id, User.GetId());
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }

            return RedirectToAction("List");
        }
    }
}

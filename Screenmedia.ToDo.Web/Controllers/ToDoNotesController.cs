using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Exceptions;
using Screenmedia.ToDo.Web.Models.ToDoNotes;
using Screenmedia.ToDo.Web.Services;

namespace Screenmedia.ToDo.Web.Controllers
{
    [Authorize]
    public class ToDoNotesController : Controller
    {
        private readonly ILogger<ToDoNotesController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IToDoNoteService _toDoNoteService;

        public ToDoNotesController(
            UserManager<IdentityUser> userManager,
            IToDoNoteService toDoNoteService,
            ILogger<ToDoNotesController> logger)
        {
            _userManager = userManager;
            _toDoNoteService = toDoNoteService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var viewModel = _toDoNoteService.Read(id, UserId);

            return View("_ToDoNote", viewModel);
        }

        [HttpGet]
        public IActionResult List()
        {
            var viewModel = _toDoNoteService.List(UserId);

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

            _toDoNoteService.Create(viewModel, UserId);

            return List();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var viewModel = _toDoNoteService.Read(id, UserId);
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
                _toDoNoteService.Update(viewModel, UserId);
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }

            return List();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _toDoNoteService.Delete(id, UserId);
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }

            return List();
        }

        private string UserId =>
            _userManager.GetUserAsync(HttpContext.User).Result.Id;
    }
}

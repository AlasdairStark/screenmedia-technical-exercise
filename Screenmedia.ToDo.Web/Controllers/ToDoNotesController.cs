using System.Threading.Tasks;
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
        public async Task<ActionResult> Get(int id)
        {
            var viewModel = _toDoNoteService.Read(id, await GetUserId().ConfigureAwait(false));

            return View("_ToDoNote", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var viewModel = _toDoNoteService.List(await GetUserId().ConfigureAwait(false));

            return View("List", viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoNoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            _toDoNoteService.Create(viewModel, await GetUserId().ConfigureAwait(false));

            return await List().ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var viewModel = _toDoNoteService.Read(id, await GetUserId().ConfigureAwait(false));
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
        public async Task<ActionResult> Update(ToDoNoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            try
            {
                _toDoNoteService.Update(viewModel, await GetUserId().ConfigureAwait(false));
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }

            return await List().ConfigureAwait(false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                _toDoNoteService.Delete(id, await GetUserId().ConfigureAwait(false));
            }
            catch (EntityNotFoundException<ToDoNote> ex)
            {
                _logger.LogDebug(ex.Message);
                return NotFound();
            }

            return await List().ConfigureAwait(false);
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            return user.Id;
        }

        //private string UserId =>
        //    _userManager.GetUserAsync(HttpContext.User).Result.Id;
    }
}

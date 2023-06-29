using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using AIS.ViewModels.TasksViewModels;
using AIS.ErrorManager;
using System.Net;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class TasksController : Controller
    {
        private readonly ILogger<ProcessController> _logger;
        private IMyTaskService _myTaskService;
        private IMyUsersService _myUserService;
        private IEnclosureService _enclosureService;
        private IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;
        IHubContext<AisHub> _hubContext;


        public TasksController(ILogger<ProcessController> logger, 
                                 IWebHostEnvironment appEnvironment, 
                                 UserManager<User> userManager, 
                                 IMyTaskService myTaskService, 
                                 IMyUsersService myUserService, 
                                 IEnclosureService enclosureService,  
                                 IHubContext<AisHub> hubContext)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _myTaskService = myTaskService;
            _myUserService = myUserService;
            _enclosureService = enclosureService;
            _hubContext = hubContext;
        }
        #region [MyTasks]
        public async Task<IActionResult> MyTasks()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var currentUser = await _myUserService.GetCurrentUser(userName);
            if (User.IsInRole("admin"))
            {
                return View(await _myTaskService.GetMyActiveTasks());
            }
            else
            {
                return View(await _myTaskService.GetMyActiveTasksWithCurrentUser(currentUser.Id));
            }
        }

        public async Task<IActionResult> TasksArchive()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var currentUser = await _myUserService.GetCurrentUser(userName);
            if (User.IsInRole("admin"))
            {
                return View(await _myTaskService.GetMyArchiveTasks());
            }
            else
            {
                return View(await _myTaskService.GetMyArchiveTasksWithCurrentUser(currentUser.Id));
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateTask()
        {
            CreateTaskViewModel model = new CreateTaskViewModel();

                var userName = User.FindFirstValue(ClaimTypes.Name);
                await model.Fill(_myUserService, _myTaskService, userName);
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskViewModel model)
        {
            var DestinationUser = await _myUserService.GetUserById(model.DestinationUserId);
            await _myTaskService.CreateTask(DestinationUser, model);
            await _hubContext.Clients.User(DestinationUser.Id).SendAsync("Notify", "Создана задача: " + model.Name + ". Автор: " + model.SenderUserName);
            return RedirectToAction("MyTasks");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMyTask(int id)
        {
            await _myTaskService.DeleteMyTask(id);
            return RedirectToAction("MyTasks");
        }

        public async Task<IActionResult> EditMyTask(int id)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            EditTaskViewModel model = new EditTaskViewModel();
            await model.Fill(id, _myTaskService, _enclosureService, _myUserService, userName);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditMyTask(EditTaskViewModel model)
        {
            var destinationUser = await _myUserService.GetUserById(model.DestinationUserId);
            await _myTaskService.EditMyTask(destinationUser, model);
            return RedirectToAction("MyTasks");
        }

        [HttpPost]
        public async Task<JsonResult> ShowCurrentTasks([FromBody] string id)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var currentUser = await _myUserService.GetCurrentUser(userName);
            
            if (User.IsInRole("admin"))
            {
                return Json(await _myTaskService.GetMyActiveTasks());
            }
            else
            {
                return Json(_myTaskService.GetMyActiveTasksWithCurrentUser(currentUser.Id));
            }
        }

        #endregion

        #region[subTasks]
        [HttpGet]
        public async Task<ActionResult> CreateSubTask(int id)
        {
            CreateSubTaskViewModel model = new CreateSubTaskViewModel();
            await model.Fill(id, _myTaskService);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubTask(CreateSubTaskViewModel model)
        {
            await _myTaskService.CreateSubTask(model);
            return RedirectToAction("EditMyTask", new { id = model.MyTaskId });
        }

        public async Task<IActionResult> EditMySubTask(int id)
        {
            EditSubTaskViewModel model = new EditSubTaskViewModel();
            await model.Fill(id, _myTaskService, _enclosureService);
            return PartialView(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditMySubTask(EditSubTaskViewModel model)
        {
            await _myTaskService.EditSubTask(model);
            return RedirectToAction("EditMyTask", new { id = model.MyTaskId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMySubTask(int id)
        {
            MySubTask currentSubTask = await _myTaskService.GetMySubTaskByIdWithFiles(id);
            await _myTaskService.DeleteMySubTask(currentSubTask);
            return RedirectToAction("EditMyTask", new { id = currentSubTask.MyTaskId });
        }

        #endregion
    }
}

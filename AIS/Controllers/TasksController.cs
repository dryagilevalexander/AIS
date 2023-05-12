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
            List<User> users;
            MyTaskViewModel myTaskViewModel = new MyTaskViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                var currentUser = await _myUserService.GetCurrentUser(userName);
                users = await _myUserService.GetUsers();
                myTaskViewModel.DestinationUsers = from destinationUser in users select new SelectListItem { Text = destinationUser.UserNickName, Value = destinationUser.Id };
                myTaskViewModel.SenderUserId = currentUser.Id;
                myTaskViewModel.SenderUserName = currentUser.UserNickName;
            }
            else
            {
                return NotFound();
            }
            var taskStatuses = await _myTaskService.GetMyTaskStatuses();
            var taskLevel = await _myTaskService.GetMyTaskLevels();
            myTaskViewModel.MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
            myTaskViewModel.MyTaskLevelImportances = from myTaskLevel in taskLevel select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };


            return View(myTaskViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(MyTaskViewModel mtvm)
        {
            var DestinationUser = await _myUserService.GetUserById(mtvm.DestinationUserId);
            await _myTaskService.CreateTask(DestinationUser, mtvm);
            await _hubContext.Clients.User(DestinationUser.Id).SendAsync("Notify", "Создана задача: " + mtvm.Name + ". Автор: " + mtvm.SenderUserName);
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
                MyTask? myTask = await _myTaskService.GetMyTaskByIdEagerLoading(id);
                if(myTask == null) return NotFound();
                IEnumerable<MyTaskStatus> myTaskStatuses = await _myTaskService.GetMyTaskStatuses();
                ViewBag.MyTaskStatuses = new SelectList(myTaskStatuses, "Id", "Name");
                IEnumerable<LevelImportance> myTaskLevels = await _myTaskService.GetMyTaskLevels();
                ViewBag.MyTaskLevels = new SelectList(myTaskLevels, "Id", "Name");
                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByTaskId(id);
                MyTaskViewModel myTaskViewModel = new MyTaskViewModel
                {
                    Id = myTask.Id,
                    Name = myTask.Name,
                    Description = myTask.Description,
                    DateStart = myTask.DateStart,
                    DateEnd = myTask.DateEnd,
                    MyTaskStatusId = myTask.MyTaskStatusId,
                    MyTaskLevelImportanceId = myTask.MyTaskLevelImportanceId,
                    MyFiles = enclosures,
                    SenderUserId = myTask.SenderUserId,
                    DestinationUserId = myTask.DestinationUserId,
                    MySubTasks = myTask.MySubTasks
                };

                string userId;
                List<User> users;
                if (User.Identity.IsAuthenticated)
                {
                    var userName = User.FindFirstValue(ClaimTypes.Name);
                    var currentUser = await _myUserService.GetCurrentUser(userName);
                    users = await _myUserService.GetUsers();
                    myTaskViewModel.DestinationUsers = from destinationUser in users select new SelectListItem { Text = destinationUser.UserNickName, Value = destinationUser.Id };
                }

             return View(myTaskViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> EditMyTask(MyTaskViewModel mtvm)
        {
            var destinationUser = await _myUserService.GetUserById(mtvm.DestinationUserId);

            await _myTaskService.EditMyTask(destinationUser, mtvm);
            return RedirectToAction("MyTasks");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMyEnclosure(int id)
        {
            await _myTaskService.DeleteMyEnclosure(id);
            return new EmptyResult();
        }
        #endregion

        #region[subTasks]
        [HttpGet]
        public async Task<ActionResult> CreateSubTask(int id)
        {
            MySubTaskViewModel mySubTaskViewModel = new MySubTaskViewModel();
            var taskStatuses = await _myTaskService.GetMyTaskStatuses();
            var taskLevel = await _myTaskService.GetMyTaskLevels();
            mySubTaskViewModel.MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
            mySubTaskViewModel.MyTaskLevelImportances = from myTaskLevel in taskLevel select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };
            mySubTaskViewModel.MyTaskId = id;

            return PartialView(mySubTaskViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubTask(MySubTaskViewModel mtvm)
        {
            await _myTaskService.CreateSubTask(mtvm);
            return RedirectToAction("EditMyTask", new { id = mtvm.MyTaskId });
        }

        public async Task<IActionResult> EditMySubTask(int? id)
        {
            if (id != null)
            {
                MySubTask? mySubTask = await _myTaskService.GetMySubTaskByIdWithFiles(id.Value);

                var taskStatuses = await _myTaskService.GetMyTaskStatusesToList();
                var taskLevels = await _myTaskService.GetMyTaskLevelsToList();

                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresBySubTaskId(id.Value);
                MySubTaskViewModel mySubTaskViewModel = new MySubTaskViewModel
                {
                    Id = mySubTask.Id,
                    MyTaskId = mySubTask.MyTaskId,
                    Name = mySubTask.Name,
                    Description = mySubTask.Description,
                    DateStart = mySubTask.DateStart,
                    DateEnd = mySubTask.DateEnd,
                    MyTaskStatusId = mySubTask.MyTaskStatusId,
                    MyTaskLevelImportanceId = mySubTask.MyTaskLevelImportanceId,
                    MyFiles = enclosures
                };

                mySubTaskViewModel.MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
                mySubTaskViewModel.MyTaskLevelImportances = from myTaskLevel in taskLevels select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };


                if (mySubTask != null) return PartialView(mySubTaskViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditMySubTask(MySubTaskViewModel mtvm)
        {
            await _myTaskService.EditSubTask(mtvm);
            return RedirectToAction("EditMyTask", new { id = mtvm.MyTaskId });
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

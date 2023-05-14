using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace AIS.ViewModels.TasksViewModels
{
    public class EditTaskViewModel
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string SenderUserName { get; set; }
        public string DestinationUserId { get; set; }
        public IEnumerable<SelectListItem>? DestinationUsers { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public IFormFileCollection? Enclosure { get; set; }
        public IEnumerable<MyFile>? MyFiles { get; set; }
        public IEnumerable<SelectListItem>? MyTaskStatuses { get; set; }
        public IEnumerable<SelectListItem>? MyTaskLevelImportances { get; set; }
        public int? MyTaskStatusId { get; set; }
        public MyTaskStatus? MyTaskStatus { get; set; }
        public int? MyTaskLevelImportanceId { get; set; }
        public LevelImportance? MyTaskLevelImportance { get; set; }
        public List<MySubTask> MySubTasks { get; set; }

        public async Task Fill(int id, IMyTaskService _myTaskService, IEnclosureService _enclosureService, IMyUsersService _myUsersService, string userName)
        {
            MyTask? myTask = await _myTaskService.GetMyTaskByIdEagerLoading(id);
            if (myTask == null) throw new AisException("Задача не найдена", HttpStatusCode.BadRequest);

            var taskStatuses = await _myTaskService.GetMyTaskStatuses();
            var taskLevel = await _myTaskService.GetMyTaskLevels();
            MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
            MyTaskLevelImportances = from myTaskLevel in taskLevel select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };


            IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByTaskId(id);

            Id = myTask.Id;
            Name = myTask.Name;
            Description = myTask.Description;
            DateStart = myTask.DateStart;
            DateEnd = myTask.DateEnd;
            MyTaskStatusId = myTask.MyTaskStatusId;
            MyTaskLevelImportanceId = myTask.MyTaskLevelImportanceId;
            MyFiles = enclosures;
            SenderUserId = myTask.SenderUserId;
            DestinationUserId = myTask.DestinationUserId;
            MySubTasks = myTask.MySubTasks;

            string userId;
            List<User> users;

            var currentUser = await _myUsersService.GetCurrentUser(userName);
            users = await _myUsersService.GetUsers();
            DestinationUsers = from destinationUser in users select new SelectListItem { Text = destinationUser.UserNickName, Value = destinationUser.Id };

        }

    }
}

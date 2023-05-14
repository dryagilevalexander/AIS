using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Security.Claims;


namespace AIS.ViewModels.TasksViewModels
{
    public class CreateTaskViewModel
    {
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

        public async Task Fill(IMyUsersService _myUserService, IMyTaskService _myTaskService, string userName)
        {
            List<User> users;
                var currentUser = await _myUserService.GetCurrentUser(userName);
                users = await _myUserService.GetUsers();
                DestinationUsers = from destinationUser in users select new SelectListItem { Text = destinationUser.UserNickName, Value = destinationUser.Id };
                SenderUserId = currentUser.Id;
                SenderUserName = currentUser.UserNickName;
                var taskStatuses = await _myTaskService.GetMyTaskStatuses();
                var taskLevel = await _myTaskService.GetMyTaskLevels();
                MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
                MyTaskLevelImportances = from myTaskLevel in taskLevel select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };
        }
    }
}

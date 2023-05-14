using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels.TasksViewModels
{
    public class EditSubTaskViewModel
    {
        public int Id { get; set; }
        public int MyTaskId { get; set; }
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

        public async Task Fill(int id, IMyTaskService _myTaskService, IEnclosureService _enclosureService)
        {
            MySubTask? mySubTask = await _myTaskService.GetMySubTaskByIdWithFiles(id);

            var taskStatuses = await _myTaskService.GetMyTaskStatusesToList();
            var taskLevels = await _myTaskService.GetMyTaskLevelsToList();

            IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresBySubTaskId(id);

            Id = mySubTask.Id;
            MyTaskId = mySubTask.MyTaskId;
            Name = mySubTask.Name;
            Description = mySubTask.Description;
            DateStart = mySubTask.DateStart;
            DateEnd = mySubTask.DateEnd;
            MyTaskStatusId = mySubTask.MyTaskStatusId;
            MyTaskLevelImportanceId = mySubTask.MyTaskLevelImportanceId;
            MyFiles = enclosures;

            MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
            MyTaskLevelImportances = from myTaskLevel in taskLevels select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };
        }
    }
}

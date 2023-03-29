using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class MyTaskViewModel
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class MyTask
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string SenderUserName { get; set; }
        public string DestinationUserId { get; set; }
        public string DestinationUserName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool FirstView { get; set; } = false;
        public int? MyTaskStatusId { get; set; }
        public MyTaskStatus? MyTaskStatus { get; set; }
        public int? MyTaskLevelImportanceId { get; set; }
        public LevelImportance? MyTaskLevelImportance { get; set; }
        public List <MyFile>? MyFiles { get; set; }
        public List <MySubTask> MySubTasks { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class MySubTask
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? MyTaskStatusId { get; set; }
        public MyTaskStatus? MyTaskStatus { get; set; }
        public int? MyTaskLevelImportanceId { get; set; }
        public LevelImportance? MyTaskLevelImportance { get; set; }
        public List<MyFile>? MyFiles { get; set; }
        public int MyTaskId { get; set; }
        public MyTask MyTask { get; set; }
    }
}

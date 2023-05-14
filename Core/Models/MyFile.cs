using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class MyFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string NameInServer { get; set; }
        public int? MyTaskId { get; set; }
        public MyTask? MyTask { get; set; }
        public int? MySubTaskId { get; set; }
        public MySubTask? MySubTask { get; set; }
        public int? ContractId { get; set; }
        public Contract? Contract { get; set; }
        public int? LetterId { get; set; }
        public Letter? Letter { get; set; }
    }
}

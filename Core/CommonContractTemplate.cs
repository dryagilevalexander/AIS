using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class CommonContractTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string? Preamble { get; set; }

        public int TypeOfDocumentId {get; set;}
        public TypeOfDocument TypeOfDocument { get; set; }
        public List<ContractTemplate> ContractTemplates { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DocumentTemplate
    {
        public int Id { get; set; }
        public string NameOfTemplate { get; set; } = string.Empty;
        public string NameOutput { get; set; } = string.Empty;
        public int? TypeOfContractId { get; set; }
        public TypeOfContract TypeOfContract { get; set; }
        public List<TypeOfStateReg>? TypeOfStateRegs { get; set; }
        public List<PartnerType>? PartnerTypes { get; set; }
        public MyFile TemplateFile { get; set; }
    }
}

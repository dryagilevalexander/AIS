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
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeOfDocumentId { get; set; }
        public TypeOfDocument TypeOfDocument { get; set; }
        public int? TypeOfContractId { get; set; }   //1 - подряд, 2 - услуги, 3 - поставка, 4 - аренда, 5 - общий шаблон 
        public TypeOfContract? TypeOfContract { get; set; }
        public List<Condition> Conditions { get; set; }
        public int RootTemplateId {get;set;}
        public RootTemplate RootTemplate { get; set; }
    }
}

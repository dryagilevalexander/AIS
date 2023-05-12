using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class RootTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeOfDocumentId {get; set;}
        public TypeOfDocument TypeOfDocument { get; set; }
        public List<DocumentTemplate> DocumentTemplates { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXML
{
    public class SubConditionParagraph
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int SubConditionId {get; set;}
        public SubCondition SubCondition { get; set; }
    }
}

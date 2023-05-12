using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SubCondition
    {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    public int NumLevelReference { get; set; }
    public int NumId { get; set; }
    public string Justification { get; set; }
    public Condition Condition { get; set; }
    public int ConditionId { get; set; }
    public List <SubConditionParagraph>? SubConditionParagraphs { get; set; }
    }
}

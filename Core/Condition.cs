using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Condition
    {
    public int Id { get; set; }
    public int? TypeOfConditionId { get; set; }
    public TypeOfCondition? TypeOfCondition { get; set; }
    public int? TypeOfStateRegId { get; set; } //1 - 44ФЗ, 2 - 223ФЗ, 3 - ГК, 4 - Все
    public TypeOfStateReg? TypeOfStateReg { get; set; }
    public int NumLevelReference {get; set;}
    public int NumId { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    public List <SubCondition>? SubConditions { get; set; }
    public int ContractTemplateId { get; set; }
    public ContractTemplate ContractTemplate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXML
{
    public class Condition
    {
    public int Id { get; set; }
    public int TypeOfCondition { get; set; } //1 - заголовок договора, 2 - преамбула, 3 - обычное условие
    public int RegulationType { get; set; } //1 - ГК, 2 - 223ФЗ, 3 - 44ФЗ, 4 - Все
    public string? Name { get; set; }
    public string? Text { get; set; }
    public List <SubCondition>? SubConditions { get; set; }
    public int ContractTemplateId { get; set; }
    public ContractTemplate ContractTemplate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXML
{
    public class ContractTemplate
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public int ContractType { get; set; }   //1 - подряд, 2 - услуги, 3 - поставка, 4 - аренда 
    public List <Condition> Conditions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXML
{
    public class DirectorType
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameR { get; set; }
    public List<Contragent> Contragents { get; set; }
    }
}

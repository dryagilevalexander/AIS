using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class PartnerOrganization: Partner
    {
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string INN { get; set; } = null!;
        public string KPP { get; set; } = null!;
        public int DirectorTypeId { get; set; }
        public DirectorType DirectorType { get; set; } = null!;
        public string DirectorName { get; set; } = null!;
        public string DirectorNameR { get; set; } = null!;
        public string Bank { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string CorrespondentAccount { get; set; } = null!;
        public string BIK { get; set; } = null!;
        public string OGRN { get; set; } = null!;
        public List<Employee>? Employeers { get; set; }
    }
}

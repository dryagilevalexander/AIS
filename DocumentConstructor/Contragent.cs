using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OpenXML
{
    public class Contragent
    {
        public int Id { get; set; }
        public bool IsMain { get; set; }
        public string Name { get; set; }
        public string? ShortName { get; set; }
        public string? Address { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public int DirectorTypeId { get; set; }
        public DirectorType DirectorType { get; set; }
        public string DirectorName { get; set; }
        public string DirectorNameR { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bank { get; set; }
        public string? Account { get; set; }
        public string? CorrespondentAccount { get; set; }
        public string? BIK { get; set; }
        public string? OGRN { get; set; }
    }
}

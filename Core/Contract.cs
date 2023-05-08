using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Contract
    {
    public int Id { get; set; }
        public string? NumberOfContract { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateStart { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }       
        public int? PartnerOrganizationId { get; set; }
        public Partner? Partner { get; set; }
        public string? SubjectOfContract { get; set; }       
        public decimal? Cost { get; set; }
        public int TypeOfContract { get; set; }
        public int? MyContractStatusId { get; set; }
        public MyContractStatus? MyContractStatus { get; set; }
        public List <MyFile>? MyFiles { get; set; } 
        public string? projectContractLink { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ContractTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeOfContractId { get; set; }   //1 - подряд, 2 - услуги, 3 - поставка, 4 - аренда, 5 - общий шаблон 
        public TypeOfContract TypeOfContract { get; set; }
        public List<Condition> Conditions { get; set; }
        public int CommonContractTemplateId {get;set;}
        public CommonContractTemplate CommonContractTemplate { get; set; }
    }
}

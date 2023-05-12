using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AIS
{
    public class DocumentModel
    {
        public int TypeOfDocumentId { get; set; }
        //Разделы документа
        public List<Infrastructure.Models.Condition> Conditions { get; set; }
        //Для договора
        public Dictionary<string, string>? CustomerProp { get; set; } //Реквизиты заказчика
        public Dictionary<string, string>? ExecutorProp { get; set; } //Реквизиты исполнителя
        //Словарь замены
        public Dictionary<string, string> ReplacementDictionary { get; set; }
    }
}

using Core;
using Microsoft.EntityFrameworkCore;

namespace AIS
{
    public class ContractService
    {
        CoreContext db;
        
        public ContractService(CoreContext context)
        {
            db = context;
        }

        //Метод получения шаблона контракта с стандартными условиями для всех типов регулирования
        public ContractTemplate GetContractTemplateId(int id)
        {
            return db.ContractTemplates.Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(c => c.SubConditionParagraphs).FirstOrDefault(p => p.Id == id);
        }

        //Метод установки условий контракта в модель контракта
        public void CreateConditions(ContractModel contract)
        {
            List<Condition> conditions = new List<Condition>();
           
            //Добавляем все условия из общего шаблона (заголовок, преамбула)
            ContractTemplate commonTemplate = GetContractTemplateId(1);
            foreach (var condition in commonTemplate.Conditions)
            {
                conditions.Add(condition);
            }

            ContractTemplate contractTemplate = GetContractTemplateId(contract.ContractTemplateId);
            foreach (var condition in contractTemplate.Conditions)
            {
                //Добавляем все общие условия
                if (condition.TypeOfStateRegId == 4)
                {
                    conditions.Add(condition);
                }
                //Если 44-ФЗ добавляем специфические условия для этого типа регулирования               
                if (contract.RegulationType == 3)
                {
                    if (condition.TypeOfStateRegId == 1)
                    {
                        conditions.Add(condition);
                    }
                }
            }
            contract.Conditions = conditions;
        }

        //Метод установки реквизитов контрагентов контракта
        public void SetContractRequisites(ContractModel contract, Partner mainOrganization, Partner contragent)
        {
            if (contract.IsCustomer == true)
            {
                contract.Customer = mainOrganization;
                contract.Executor = contragent;
                contract.CustomerProp = GetRequisites(mainOrganization);
                contract.ExecutorProp = GetRequisites(contragent);
            }
            else
            {
                contract.Customer = contragent;
                contract.Executor = mainOrganization;
                contract.CustomerProp = GetRequisites(contragent);
                contract.ExecutorProp = GetRequisites(mainOrganization);
            }
        }

        //Метод получения реквизитов
        private Dictionary<string, string> GetRequisites(Partner contragent)
        {
            Dictionary<string, string> props = new Dictionary<string, string>()
            {
                    {contragent.ShortName,""},
                    {"ИНН", contragent.INN},
                    {"КПП", contragent.KPP},
                    {"ОГРН", contragent.OGRN},
                    {"Адрес", contragent.Address},
                    {"Банк", contragent.Bank},
                    {"БИК", contragent.BIK},
                    {"р/с", contragent.Account},
                    {"к/с", contragent.CorrespondentAccount},
                    {contragent.DirectorType.Name + " _________ ", contragent.DirectorName}
            };
            return props;
        }
    }
}

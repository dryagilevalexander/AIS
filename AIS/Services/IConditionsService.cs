using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface IConditionsService
    {
        Task<List<ContractTemplate>> GetContractTemplates();
        Task<bool> CreateContractTemplate(ContractTemplateViewModel ctvm);
        Task<bool> EditContractTemplate(ContractTemplateViewModel ctvm);
        Task<ContractTemplate?> GetContractTemplateWithConditionsById(int id);
        Task<List<TypeOfCondition>> GetTypesOfCondition();
        Task<bool> DeleteContractTemplate(int? id);
        Task<List<Condition>> GetConditions();
        Task<Condition?> GetCondition(int id);
        Task<bool> CreateCondition(ConditionViewModel cvm);
        Task<bool> EditCondition(ConditionViewModel cvm);
        Task<bool> DeleteCondition(int? id);
        Task<bool> CreateSubCondition(SubConditionViewModel scvm);
        Task<bool> DeleteSubCondition(int? id);
        Task<SubCondition?> GetSubCondition(int id);
    }
}

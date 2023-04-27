﻿using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface IConditionsService
    {
        Task<List<ContractTemplate>> GetContractTemplates();
        Task<bool> CreateContractTemplate(ContractTemplateViewModel ctvm);
        Task<bool> EditContractTemplate(ContractTemplateViewModel ctvm);
        Task<ContractTemplate?> GetContractTemplateWithConditionsById(int id);
        Task<ContractTemplate?> GetContractTemplateWithTypeOfContractById(int id);
        Task<List<TypeOfCondition>> GetTypesOfCondition();
        Task<bool> DeleteContractTemplate(int? id);
        Task<List<Condition>> GetConditions();
        Task<Condition?> GetCondition(int id);
        Task<bool> CreateCondition(ConditionViewModel cvm);
        Task<bool> EditCondition(ConditionViewModel cvm);
        Task<bool> DeleteCondition(int? id);
        Task<bool> CreateSubCondition(SubConditionViewModel scvm);
        Task<bool> EditSubCondition(SubConditionViewModel scvm);
        Task<bool> DeleteSubCondition(int? id);
        Task<bool> CreateSubConditionParagraph(SubConditionParagraphViewModel scpvm);
        Task<bool> EditSubConditionParagraph(SubConditionParagraphViewModel scpvm);
        Task<bool> DeleteSubConditionParagraph(int? id);
        Task<SubCondition?> GetSubCondition(int id);
        Task<SubConditionParagraph?> GetSubConditionParagraph(int id);
    }
}
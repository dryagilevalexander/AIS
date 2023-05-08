using AIS.ViewModels.DocumentsViewModels;
using Core;

namespace AIS.Services
{
    public interface IConditionsService
    {
        Task<List<RootTemplate>> GetRootTemplates();
        Task<List<DocumentTemplate>> GetDocumentTemplates();
        Task<bool> CreateRootTemplate(RootTemplateViewModel cctvm);
        Task<bool> EditRootTemplate(RootTemplateViewModel cctvm);
        Task<bool> DeleteRootTemplate(int? id);
        Task<RootTemplate> GetRootTemplateById(int id);
        Task<RootTemplate> GetRootTemplateWithDocumentTemplatesById(int id);
        Task<bool> CreateDocumentTemplate(DocumentTemplateViewModel ctvm);
        Task<bool> EditDocumentTemplate(DocumentTemplateViewModel ctvm);
        Task<DocumentTemplate?> GetDocumentTemplateById(int id);
        DocumentTemplate GetDocumentTemplateEagerLoadingById(int id);
        Task<DocumentTemplate?> GetDocumentTemplateWithConditionsById(int id);
        Task<DocumentTemplate?> GetDocumentTemplateWithTypeOfContractById(int id);
        Task<DocumentTemplate?> GetDocumentTemplateWithRootTemplateById(int id);
        Task<bool> DeleteDocumentTemplate(int? id);
        Task<List<Condition>> GetConditions();
        Task<Condition?> GetCondition(int id);
        Task<int> GetNumberOfConditionsInDocumentTemplate(int id);
        Task<Condition?> GetConditionByNumberInDocument(int number);
        Task<bool> CreateCondition(ConditionViewModel cvm);
        Task<bool> EditCondition(ConditionViewModel cvm);
        Task<bool> DeleteCondition(int? id);
        Task<bool> SaveCondition(Condition condition);
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

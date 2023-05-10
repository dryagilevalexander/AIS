using AIS.ViewModels.DocumentsViewModels;
using Core;

namespace AIS.Services
{
    public interface IConditionsService
    {
        Task<List<RootTemplate>> GetRootTemplates();
        Task<List<DocumentTemplate>> GetDocumentTemplates();
        Task CreateRootTemplate(RootTemplateViewModel cctvm);
        Task EditRootTemplate(RootTemplateViewModel cctvm);
        Task DeleteRootTemplate(int id);
        Task<RootTemplate> GetRootTemplateById(int id);
        Task<RootTemplate> GetRootTemplateWithDocumentTemplatesById(int id);
        Task CreateDocumentTemplate(DocumentTemplateViewModel ctvm);
        Task EditDocumentTemplate(DocumentTemplateViewModel ctvm);
        Task<DocumentTemplate> GetDocumentTemplateById(int id);
        DocumentTemplate GetDocumentTemplateEagerLoadingById(int id);
        Task<DocumentTemplate> GetDocumentTemplateWithConditionsById(int id);
        Task<DocumentTemplate> GetDocumentTemplateWithTypeOfContractById(int id);
        Task<DocumentTemplate> GetDocumentTemplateWithRootTemplateById(int id);
        Task DeleteDocumentTemplate(int id);
        Task<List<Condition>> GetConditions();
        Task<Condition> GetCondition(int id);
        Task<int> GetNumberOfConditionsInDocumentTemplate(int id);
        Task<Condition> GetConditionByNumberInDocument(int number);
        Task CreateCondition(ConditionViewModel cvm);
        Task EditCondition(ConditionViewModel cvm);
        Task DeleteCondition(int id);
        Task SaveCondition(Condition condition);
        Task CreateSubCondition(SubConditionViewModel scvm);
        Task EditSubCondition(SubConditionViewModel scvm);
        Task DeleteSubCondition(int id);
        Task CreateSubConditionParagraph(SubConditionParagraphViewModel scpvm);
        Task EditSubConditionParagraph(SubConditionParagraphViewModel scpvm);
        Task DeleteSubConditionParagraph(int id);
        Task<SubCondition> GetSubCondition(int id);
        Task<SubConditionParagraph?> GetSubConditionParagraph(int id);
    }
}

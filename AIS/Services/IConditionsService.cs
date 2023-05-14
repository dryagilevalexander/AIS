using AIS.ViewModels.DocumentsViewModels;
using Infrastructure;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface IConditionsService
    {
        Task<List<RootTemplate>> GetRootTemplates();
        Task<List<DocumentTemplate>> GetDocumentTemplates();
        Task CreateRootTemplate(CreateRootTemplateViewModel model);
        Task EditRootTemplate(EditRootTemplateViewModel model);
        Task DeleteRootTemplate(int id);
        Task<RootTemplate> GetRootTemplateById(int id);
        Task<RootTemplate> GetRootTemplateWithDocumentTemplatesById(int id);
        Task CreateDocumentTemplate(CreateDocumentTemplateViewModel model);
        Task EditDocumentTemplate(EditDocumentTemplateViewModel model);
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
        Task CreateCondition(CreateConditionViewModel model);
        Task EditCondition(EditConditionViewModel model);
        Task DeleteCondition(int id);
        Task SaveCondition(Condition condition);
        Task CreateSubCondition(CreateSubConditionViewModel model);
        Task EditSubCondition(EditSubConditionViewModel model);
        Task DeleteSubCondition(int id);
        Task CreateSubConditionParagraph(CreateSubConditionParagraphViewModel model);
        Task EditSubConditionParagraph(EditSubConditionParagraphViewModel model);
        Task DeleteSubConditionParagraph(int id);
        Task<SubCondition> GetSubCondition(int id);
        Task<SubConditionParagraph?> GetSubConditionParagraph(int id);
        Task<List<DocumentTemplate>> GetDocumentTemplatesByTypeOfDocumentId(int id);
    }
}

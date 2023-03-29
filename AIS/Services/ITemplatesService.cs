using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface ITemplatesService
    {
        Task<DocumentTemplate> GetDocumentTemplateWithFilesById(int id);
        Task<DocumentTemplate> GetTemplateByIdEagerLoading(int id);
        Task<IEnumerable<DocumentTemplate>> GetAllTemplatesWitnToFAndToS();
        Task<IEnumerable<DocumentTemplate>> GetTemplatesWithTypeOfContractAndPartnerType(int typeOfContractId, int partnerTypeId);
        Task<bool> DeleteTemplate(int? id);
        Task<bool> CreateTemplate(TemplateViewModel tvm);
    }
}

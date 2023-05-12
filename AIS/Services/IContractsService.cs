using AIS.ViewModels.ContractsViewModels;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface IContractsService
    {
        Task<List<Contract>> GetActiveContractsEagerLoading();
        Task<List<Contract>> GetArchiveContractsEagerLoading();
        Task<IEnumerable<TypeOfContract>> GetTypeOfContracts();
        Task<TypeOfStateReg> GetTypeOfStateRegById(int id);
        Task<IEnumerable<TypeOfStateReg>> GetTypeOfStateRegs();
        Task<IEnumerable<ArticleOfLaw>> GetArticleOfLaws();
        Task<IEnumerable<MyContractStatus>> GetMyContractStatuses();
        Task CreateContract(MyContractViewModel mcvm, int typeOfContract);
        Task EditContract(MyContractViewModel mcvm);
        Task DeleteContract(int id);
        Task<Contract> GetContractByIdWithMyFiles(int id);
        DocumentModel SetContractRequisites(DocumentModel contract, bool isCustomer, Partner mainOrganization, Partner contragent);
        Task<IEnumerable<TypeOfDocument>> GetTypesOfDocument();
    }
}

﻿using AIS.ViewModels;
using Core;

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
        Task<bool> CreateContract(MyContractViewModel mcvm);
        Task<bool> EditContract(MyContractViewModel mcvm);
        Task<bool> DeleteContract(int? id);
        Task<Contract> GetContractByIdWithMyFiles(int id);
    }
}

using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using AIS.ViewModels.ContractsViewModels;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class ContractsController : Controller
    {
        private IPartnerService _partnerService;
        private IContractsService _contractsService;
        private IEnclosureService _enclosureService;
        private IConditionsService _conditionsService;


        public ContractsController(IPartnerService partnerService, 
                                 IContractsService contractsService, 
                                 IEnclosureService enclosureService, 
                                 IConditionsService conditionsService, 
                                 IHubContext<AisHub> hubContext)
        {
            _partnerService = partnerService;
            _contractsService = contractsService;
            _enclosureService = enclosureService;
            _conditionsService = conditionsService;
        }

        #region [Contracts]
        public async Task<IActionResult> MyContracts()
        {
            return View(await _contractsService.GetActiveContractsEagerLoading());
        }

        public async Task<IActionResult> ContractsArchive()
        {
            return View(await _contractsService.GetArchiveContractsEagerLoading());
        }

        public async Task<IActionResult> CreateContract()
        {
            MyContractViewModel myContractViewModel = new MyContractViewModel();
            IEnumerable<Partner> myPartners = await _partnerService.GetPartnersWithoutOurOrganization();
            myContractViewModel.MyPartners = from myPartner in myPartners select new SelectListItem { Text = myPartner.ShortName, Value = myPartner.Id.ToString() };
            IEnumerable<DocumentTemplate> DocumentTemplates = await _conditionsService.GetDocumentTemplates();
            IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
            IEnumerable<ArticleOfLaw> articleOfLaws = await _contractsService.GetArticleOfLaws();
            IEnumerable<MyContractStatus> myContractStatuses = await _contractsService.GetMyContractStatuses();
            myContractViewModel.DocumentTemplates = from DocumentTemplate in DocumentTemplates select new SelectListItem { Text = DocumentTemplate.Name, Value = DocumentTemplate.Id.ToString() };
            myContractViewModel.TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
            myContractViewModel.ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };
            myContractViewModel.MyContractStatuses = from statusOfContract in myContractStatuses select new SelectListItem { Text = statusOfContract.Name, Value = statusOfContract.Id.ToString() };
            return View(myContractViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(MyContractViewModel mcvm)
        {

             if (!ModelState.IsValid)
             {
                return NotFound();
             }

            DocumentTemplate DocumentTemplate = await _conditionsService.GetDocumentTemplateWithTypeOfContractById(Convert.ToInt32(mcvm.DocumentTemplateId));
            int typeOfContractId = DocumentTemplate.TypeOfContractId.Value;

            _contractsService.CreateContract(mcvm, typeOfContractId);
             return RedirectToAction("MyContracts");
        }

        public async Task<IActionResult> EditContract(int id)
        {
             if (!ModelState.IsValid)
             {
                 return NotFound();
             }
            
                Contract? contract = await _contractsService.GetContractByIdWithMyFiles(id);
                if(contract == null) return NotFound();

                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByContractId(id);
                IEnumerable<Partner> myPartners = await _partnerService.GetPartners();
                var partners = from myPartner in myPartners select new SelectListItem { Text = myPartner.ShortName, Value = myPartner.Id.ToString() };
                IEnumerable<MyContractStatus> myContractStatuses = await _contractsService.GetMyContractStatuses();
                var contractStatuses = from statusOfContract in myContractStatuses select new SelectListItem { Text = statusOfContract.Name, Value = statusOfContract.Id.ToString() };
                IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
                IEnumerable<ArticleOfLaw> articleOfLaws = await _contractsService.GetArticleOfLaws();

                MyContractViewModel myContractViewModel = new MyContractViewModel
                {
                    Id = contract.Id,
                    NumberOfContract = contract.NumberOfContract,
                    DateStart = contract.DateStart,
                    DateEnd = contract.DateEnd,
                    PartnerOrganizationId = contract.PartnerOrganizationId,
                    SubjectOfContract = contract.SubjectOfContract,
                    Cost = (decimal)contract.Cost,
                    MyPartners = partners,
                    MyContractStatuses = contractStatuses,
                    MyContractStatusId = contract.MyContractStatusId,
                    MyFiles = enclosures
                };

                myContractViewModel.TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
                myContractViewModel.ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };


                return View(myContractViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditContract(MyContractViewModel mcvm)
        {
            await _contractsService.EditContract(mcvm);
            return RedirectToAction("MyContracts");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteContract(int id)
        {
            await _contractsService.DeleteContract(id);
            return RedirectToAction("MyContracts");
        }
        #endregion
    }
}

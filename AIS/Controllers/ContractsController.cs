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
            CreateContractViewModel model = new CreateContractViewModel();
            await model.Fill(_partnerService, _conditionsService, _contractsService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(CreateContractViewModel model)
        {

             if (!ModelState.IsValid)
             {
                return NotFound();
             }

            DocumentTemplate DocumentTemplate = await _conditionsService.GetDocumentTemplateWithTypeOfContractById(model.DocumentTemplateId);
            int typeOfContractId = DocumentTemplate.TypeOfContractId.Value;

            await _contractsService.CreateContract(model, typeOfContractId);
            return RedirectToAction("MyContracts");
        }

        public async Task<IActionResult> EditContract(int id)
        {
             if (!ModelState.IsValid)
             {
                 return NotFound();
             }

             EditContractViewModel model = new EditContractViewModel();
             await model.Fill(id, _contractsService, _enclosureService, _partnerService);
             return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditContract(EditContractViewModel model)
        {
            await _contractsService.EditContract(model);
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

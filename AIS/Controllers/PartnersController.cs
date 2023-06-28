using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AIS.ViewModels.PartnersViewModels;
using AIS.ErrorManager;
using System.Net;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class PartnersController : Controller
    {
        private readonly ILogger<ProcessController> _logger;
        private IPartnerService _partnerService;
        private IWebHostEnvironment _appEnvironment;


        public PartnersController(ILogger<ProcessController> logger, 
                                 IWebHostEnvironment appEnvironment, 
                                 IPartnerService partnerService)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _partnerService = partnerService;
        }
        #region [Partners]
        public async Task<IActionResult> Partners()
        {
            PartnersViewModel model = new PartnersViewModel();
            await model.Fill(_partnerService);
            return View(model);
        }

        public async Task<IActionResult> CreatePartner()
        {
            СreatePartnerViewModel model = new СreatePartnerViewModel();
            await model.Fill(_partnerService);
            return View(model);
        }

        public async Task<IActionResult> CreatePartnerOrganization()
        {
            CreatePartnerOrganizationViewModel model = new CreatePartnerOrganizationViewModel();
            await model.Fill(_partnerService);
            return PartialView(model);
        }

        public async Task<JsonResult> FillInAutomaticallyPartnerOrganization(string id)
        {
            Partner partner = new Partner();
            partner = await _partnerService.GetPartnerInformation(id);
            return Json(partner);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartnerOrganization(CreatePartnerOrganizationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _partnerService.CreatePartnerOrganization(model);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditPartner(int id)
        {
            Partner partner = await _partnerService.GetPartner(id);
            int partnerTypeId = partner.PartnerTypeId;
            if (partnerTypeId == 1)
            {
                return RedirectToAction("EditPartnerOrganization", new { id = id});
            } else if (partnerTypeId == 2)
            {
                return RedirectToAction("EditPartnerIp", new { id = id });
            }
            else if (partnerTypeId == 3)
            {
                return RedirectToAction("EditPartnerFl", new { id = id });
            }
            return NotFound();
        }

        public async Task<IActionResult> EditPartnerOrganization(int id)
        {
            EditPartnerOrganizationViewModel model = new EditPartnerOrganizationViewModel();
            await model.Fill(id, _partnerService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPartnerOrganization(EditPartnerOrganizationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _partnerService.EditPartnerOrganization(model);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> CreatePartnerIp()
        {
            CreatePartnerIpViewModel model = new CreatePartnerIpViewModel();
            await model.Fill(_partnerService);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartnerIp(CreatePartnerIpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
          
            await _partnerService.CreatePartnerIp(model);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditPartnerIp(int id)
        {
            EditPartnerIpViewModel model = new EditPartnerIpViewModel();
            await model.Fill(id, _partnerService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPartnerIp(EditPartnerIpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _partnerService.EditPartnerIp(model);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> CreatePartnerFl()
        {
            CreatePartnerFlViewModel model = new CreatePartnerFlViewModel();
            await model.Fill(_partnerService);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartnerFl(CreatePartnerFlViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _partnerService.CreatePartnerFl(model);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditPartnerFl(int id)
        {
            EditPartnerFlViewModel model = new EditPartnerFlViewModel();
            await model.Fill(id, _partnerService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPartnerFl(EditPartnerFlViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _partnerService.EditPartnerFl(model);
            return RedirectToAction("Partners");
        }


        [HttpPost]
        public async Task<IActionResult> DeletePartner(int id)
        {
            await _partnerService.DeletePartner(id);
            return RedirectToAction("Partners");
        }

       // public async Task<IActionResult> PartnerCard(int id)
       // {
           // return PartialView(await _partnerService.GetPartnerEagerLoading(id));
       // }
        #endregion
    }
}

using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AIS.ViewModels.PartnersViewModels;

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
            PartnersViewModel pvm = new PartnersViewModel();
            IEnumerable<Partner> partners = await _partnerService.GetPartnersEagerLoading();
            List<PartnerModel> partnersData = new List<PartnerModel>();
            foreach (Partner partner in partners)
            {
                PartnerModel partnerModel = new AIS.ViewModels.PartnersViewModels.PartnerModel();

                    partnerModel.Id = partner.Id;
                    if (partner.PartnerTypeId == 1) partnerModel.Name = partner.Name;
                    else partnerModel.Name = partner.Fio; 
                    partnerModel.Address = partner.Address;
                    partnerModel.Email = partner.Email;
                    partnerModel.PhoneNumber = partner.PhoneNumber;
                    partnerModel.PartnerType = partner.PartnerType;


                partnersData.Add(partnerModel);
            }
            pvm.Partners = partnersData.ToList();
            return View(pvm);
        }

        public async Task<IActionResult> CreatePartner()
        {
            СreatePartnerViewModel model = new СreatePartnerViewModel();
            var partnerTypes = await _partnerService.GetPartnerTypes();
            model.PartnerTypes = from partnerType in partnerTypes select new SelectListItem { Text = partnerType.Name, Value = partnerType.Id.ToString() };

            return View(model);
        }

        public async Task<IActionResult> CreatePartnerOrganization()
        {
            CreatePartnerOrganizationViewModel model = new CreatePartnerOrganizationViewModel();
            var directorTypes = await _partnerService.GetDirectorTypes();
            var categories = await _partnerService.GetCategories();
            var partnerStatuses = await _partnerService.GetPartnerStatuses();

            model.DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            model.PartnerCategories = from category in categories select new SelectListItem { Text = category.Name, Value = category.Id.ToString() };
            model.DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            model.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            model.PartnerTypeId = 1;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartnerOrganization(CreatePartnerOrganizationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            Partner partner = new Partner()
            {
                Name = model.Name,
                ShortName = model.ShortName,
                INN = model.INN,
                KPP = model.KPP,
                DirectorTypeId = model.DirectorTypeId,
                DirectorName = model.DirectorName,
                DirectorNameR = model.DirectorNameR,
                Bank = model.Bank,
                Account = model.Account,
                CorrespondentAccount = model.CorrespondentAccount,
                BIK = model.BIK,
                OGRN = model.OGRN,
                PartnerCategoryId = model.PartnerCategoryId,
                PartnerTypeId = model.PartnerTypeId,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email                             
            };
                await _partnerService.CreatePartner(partner);
                return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditPartner(int id)
        {
            Partner? partner = await _partnerService.GetPartner(id);
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
            Partner? partner = await _partnerService.GetPartner(id);
            List<Employee> employees = await _partnerService.GetEmployeesByPartnerId(id);
            EditPartnerOrganizationViewModel model = new EditPartnerOrganizationViewModel()
            {
                Id = partner.Id,
                Name = partner.Name,
                ShortName = partner.ShortName,
                INN = partner.INN,
                KPP = partner.KPP,
                DirectorTypeId = partner.DirectorTypeId.Value,
                DirectorName = partner.DirectorName,
                DirectorNameR = partner.DirectorNameR,
                Bank = partner.Bank,
                Account = partner.Account,
                CorrespondentAccount = partner.CorrespondentAccount,
                BIK = partner.BIK,
                OGRN = partner.OGRN,
                PartnerCategoryId = partner.PartnerCategoryId.Value,
                PartnerTypeId = partner.PartnerTypeId,
                Address = partner.Address,
                PhoneNumber = partner.PhoneNumber,
                Email = partner.Email,
                Employees = employees
            };
            
            var directorTypes = await _partnerService.GetDirectorTypes();
            var categories = await _partnerService.GetCategories();
            var partnerStatuses = await _partnerService.GetPartnerStatuses();

            model.DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            model.PartnerCategories = from category in categories select new SelectListItem { Text = category.Name, Value = category.Id.ToString() };
            model.DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            model.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            model.PartnerTypeId = 1;

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
            var partnerStatuses = await _partnerService.GetPartnerStatuses();
            model.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            model.PartnerTypeId = 2;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartnerIp(CreatePartnerIpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            Partner partner = new Partner()
            {
                PartnerTypeId = model.PartnerTypeId,
                Fio = model.Fio,
                ShortFio = model.ShortFio,
                ShortFioR = model.ShortFioR,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                INN = model.INN,
                PartnerStatusId = model.PartnerStatusId,
                Bank = model.Bank,
                Account = model.Account,
                CorrespondentAccount = model.CorrespondentAccount,
                BIK = model.BIK,
                PassportSeries = model.PassportSeries,
                PassportNumber = model.PassportNumber,
                PassportDateOfIssue = model.PassportDateOfIssue,
                PassportDateOfBirth = model.PassportDateOfBirth,
                PassportPlaseOfIssue = model.PassportPlaseOfIssue,
                PassportDivisionCode = model.PassportDivisionCode
            };
            await _partnerService.CreatePartner(partner);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditPartnerIp(int id)
        {
            Partner? partner = await _partnerService.GetPartner(id);
            if (partner == null) return NotFound();
            EditPartnerIpViewModel model = new EditPartnerIpViewModel()
            {
                Id = partner.Id,
                Fio = partner.Fio,
                ShortFio = partner.ShortFio,
                ShortFioR = partner.ShortFioR,
                Address = partner.Address,
                Email = partner.Email,
                PhoneNumber = partner.PhoneNumber,
                INN = partner.INN,
                PartnerStatusId = partner.PartnerStatusId,
                Bank = partner.Bank,
                Account = partner.Account,
                CorrespondentAccount = partner.CorrespondentAccount,
                BIK = partner.BIK,
                PassportSeries = partner.PassportSeries,
                PassportNumber = partner.PassportNumber,
                PassportDateOfIssue = partner.PassportDateOfIssue,
                PassportDateOfBirth = partner.PassportDateOfBirth,
                PassportPlaseOfIssue = partner.PassportPlaseOfIssue,
                PassportDivisionCode = partner.PassportDivisionCode
            };

            var partnerStatuses = await _partnerService.GetPartnerStatuses();
            model.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            model.PartnerTypeId = 2;

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
            var partnerStatuses = await _partnerService.GetPartnerStatuses();
            model.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            model.PartnerTypeId = 3;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartnerFl(CreatePartnerIpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            Partner partner = new Partner()
            {
                PartnerTypeId = model.PartnerTypeId,
                Fio = model.Fio,
                ShortFio = model.ShortFio,
                ShortFioR = model.ShortFioR,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                INN = model.INN,
                PartnerStatusId = model.PartnerStatusId,
                PassportSeries = model.PassportSeries,
                PassportNumber = model.PassportNumber,
                PassportDateOfIssue = model.PassportDateOfIssue,
                PassportDateOfBirth = model.PassportDateOfBirth,
                PassportPlaseOfIssue = model.PassportPlaseOfIssue,
                PassportDivisionCode = model.PassportDivisionCode
            };
            await _partnerService.CreatePartner(partner);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditPartnerFl(int id)
        {
            Partner? partner = await _partnerService.GetPartner(id);
            if (partner == null) return NotFound();
            EditPartnerFlViewModel model = new EditPartnerFlViewModel()
            {
                Id = partner.Id,
                Fio = partner.Fio,
                ShortFio = partner.ShortFio,
                ShortFioR = partner.ShortFioR,
                Address = partner.Address,
                Email = partner.Email,
                PhoneNumber = partner.PhoneNumber,
                INN = partner.INN,
                PartnerStatusId = partner.PartnerStatusId,
                PassportSeries = partner.PassportSeries,
                PassportNumber = partner.PassportNumber,
                PassportDateOfIssue = partner.PassportDateOfIssue,
                PassportDateOfBirth = partner.PassportDateOfBirth,
                PassportPlaseOfIssue = partner.PassportPlaseOfIssue,
                PassportDivisionCode = partner.PassportDivisionCode
            };

            var partnerStatuses = await _partnerService.GetPartnerStatuses();
            model.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            model.PartnerTypeId = 3;

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

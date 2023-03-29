using Microsoft.AspNetCore.Mvc;
using AIS.Models;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.ViewModels;
using AIS.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class ProcessController : Controller
    {
        private readonly ILogger<ProcessController> _logger;
        private IPartnerService _partnerService;
        private IEmployeeService _employeeService;
        private IMyTaskService _myTaskService;
        private IMyUsersService _myUserService;
        private IContractsService _contractsService;
        private ITemplatesService _templatesService;
        private ILetterService _letterService;
        private IEnclosureService _enclosureService;
        private IWebHostEnvironment _appEnvironment;
        private IDocumentConstructor _documentConstructor;
        private readonly UserManager<User> _userManager;
        IHubContext<AisHub> _hubContext;


        public ProcessController(ILogger<ProcessController> logger, IWebHostEnvironment appEnvironment, IDocumentConstructor documentConstructor, UserManager<User> userManager, IPartnerService partnerService, IEmployeeService employeeService, IMyTaskService myTaskService, IMyUsersService myUserService, IContractsService contractsService, ITemplatesService templatesService, ILetterService letterService, IEnclosureService enclosureService, IHubContext<AisHub> hubContext)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _documentConstructor = documentConstructor;
            _userManager = userManager;
            _employeeService = employeeService;
            _partnerService = partnerService;
            _myTaskService = myTaskService;
            _myUserService = myUserService;
            _contractsService = contractsService;
            _templatesService = templatesService;
            _letterService = letterService;
            _enclosureService = enclosureService;
            _hubContext = hubContext;
        }


        #region [Employeers]
        public async Task<IActionResult> Employeers()
        {
            return View(await _employeeService.GetEmployeers());
        }

        public async Task<IActionResult> CreateEmployee()
        {
            IEnumerable<Partner> partners = await _partnerService.GetPartners();
            if (partners is not null)
            {
                ViewBag.Partners = new SelectList(partners, "Id", "Name");
                return View();
            }
            else
            { 
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (await _employeeService.CreateEmployee(employee) == true)
            {
                return RedirectToAction("Employeers");
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (await _employeeService.DeleteEmployee(id) == true)
            {
                return RedirectToAction("Employeers");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (id != null)
            {
                IEnumerable<Partner> partners = await _partnerService.GetPartners();
                ViewBag.Partners = new SelectList(partners, "Id", "Name");
                Employee? employee = await _employeeService.GetEmployee(id.Value);
                if (employee != null) return View(employee);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            if (await _employeeService.EditEmployee(employee) == true)
            {
                return RedirectToAction("Employeers");
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region [Partners]
        public async Task<IActionResult> Partners()
        {
            return View(await _partnerService.GetPartnersEagerLoading());
        }

        public async Task<IActionResult> CreatePartner()
        {
            PartnerViewModel partnerViewModel = new PartnerViewModel();
            var directorTypes = await _partnerService.GetDirectorTypes();
            partnerViewModel.DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            var partnerStatuses = await _partnerService.GetPartnerStatuses();
            partnerViewModel.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            var partnerTypes = await _partnerService.GetPartnerTypes();
            partnerViewModel.PartnerTypes = from partnerType in partnerTypes select new SelectListItem { Text = partnerType.Name, Value = partnerType.Id.ToString() };

            return View(partnerViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePartner(PartnerViewModel partnerViewModel)
        {
            if(await _partnerService.CreatePartner(partnerViewModel)== true)
            {
                return RedirectToAction("Partners");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePartner(int? id)
        {
            if (await _partnerService.DeletePartner(id) == true)
            {
                return RedirectToAction("Partners");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditPartner(int? id)
        {
            if (id != null)
            {
                Partner? partner = await _partnerService.GetPartner(id.Value);
                var partnerStatuses = await _partnerService.GetPartnerStatuses();
                var directorTypes = await _partnerService.GetDirectorTypes();

                PartnerViewModel partnerViewModel = new PartnerViewModel
                {
                    Id = partner.Id,
                    Name = partner.Name,
                    ShortName = partner.ShortName,
                    Address = partner.Address,
                    Email = partner.Email,
                    PhoneNumber = partner.PhoneNumber,
                    INN = partner.INN,
                    KPP = partner.KPP,
                    OGRN = partner.OGRN,
                    Bank = partner.Bank,
                    Account = partner.Account,
                    CorrespondentAccount = partner.CorrespondentAccount,
                    BIK = partner.BIK,
                    DirectorTypeId = partner.DirectorTypeId,
                    DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() },
                    DirectorName = partner.DirectorName,
                    PartnerStatusId = partner.PartnerStatusId,
                    PartnerTypeId = partner.PartnerTypeId,
                    PassportSeries = partner.PassportSeries,
                    PassportNumber = partner.PassportNumber,
                    PassportDateOfIssue = partner.PassportDateOfIssue,
                    PassportDateOfBirth = partner.PassportDateOfBirth,
                    PassportPlaseOfIssue = partner.PassportPlaseOfIssue,
                    PassportDivisionCode = partner.PassportDivisionCode
                };

                partnerViewModel.PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };

                if (partner != null) return View(partnerViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditPartner(Partner partner)
        {
            if (await _partnerService.EditPartner(partner) == true)
            {
                return RedirectToAction("Partners");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> PartnerCard(int id)
        {
            return PartialView(await _partnerService.GetPartnerEagerLoading(id));
        }
        #endregion

        #region [MyTasks]
        public async Task<IActionResult> MyTasks()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var currentUser = await _myUserService.GetCurrentUser(userName);
            if(User.IsInRole("admin"))
            {
                return View(await _myTaskService.GetMyActiveTasks());
            }
            else
            { 
                return View(await _myTaskService.GetMyActiveTasksWithCurrentUser(currentUser.Id));
            }
        }

        public async Task<IActionResult> TasksArchive()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var currentUser = await _myUserService.GetCurrentUser(userName);
            if (User.IsInRole("admin"))
            {
                return View(await _myTaskService.GetMyArchiveTasks());
            }
            else
            {
                return View(await _myTaskService.GetMyArchiveTasksWithCurrentUser(currentUser.Id));
            }
        }

        public async Task<IActionResult> CreateTask()
        {
            List<User> users;
            MyTaskViewModel myTaskViewModel = new MyTaskViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                var currentUser = await _myUserService.GetCurrentUser(userName);
                users = await _myUserService.GetUsers();
                myTaskViewModel.DestinationUsers = from destinationUser in users select new SelectListItem { Text = destinationUser.UserNickName, Value = destinationUser.Id };
                myTaskViewModel.SenderUserId = currentUser.Id;
                myTaskViewModel.SenderUserName = currentUser.UserNickName;
            }
            else
            {
                return NotFound();
            }
            var taskStatuses = await _myTaskService.GetMyTaskStatuses();
            var taskLevel = await _myTaskService.GetMyTaskLevels();
            myTaskViewModel.MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
            myTaskViewModel.MyTaskLevelImportances = from myTaskLevel in taskLevel select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };


            return View(myTaskViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(MyTaskViewModel mtvm)
        {
            var DestinationUser = await _myUserService.GetUserById(mtvm.DestinationUserId);
            if (await _myTaskService.CreateTask(DestinationUser, mtvm) == true)
            {
                await _hubContext.Clients.User(DestinationUser.Id).SendAsync("Notify", "Создана задача: " + mtvm.Name + ". Автор: " + mtvm.SenderUserName);
                return RedirectToAction("MyTasks");
            }
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMyTask(int? id)
        {
            if (await _myTaskService.DeleteMyTask(id) == true)
            {
                return RedirectToAction("MyTasks");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditMyTask(int? id)
        {
            if (id != null)
            {
                MyTask? myTask = await _myTaskService.GetMyTaskByIdEagerLoading(id.Value);
                IEnumerable<MyTaskStatus> myTaskStatuses = await _myTaskService.GetMyTaskStatuses();
                ViewBag.MyTaskStatuses = new SelectList(myTaskStatuses, "Id", "Name");
                IEnumerable<LevelImportance> myTaskLevels = await _myTaskService.GetMyTaskLevels();
                ViewBag.MyTaskLevels = new SelectList(myTaskLevels, "Id", "Name");
                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByTaskId(id.Value);
                MyTaskViewModel myTaskViewModel = new MyTaskViewModel
                {
                    Id = myTask.Id,
                    Name = myTask.Name,
                    Description = myTask.Description,
                    DateStart = myTask.DateStart,
                    DateEnd = myTask.DateEnd,
                    MyTaskStatusId = myTask.MyTaskStatusId,
                    MyTaskLevelImportanceId = myTask.MyTaskLevelImportanceId,
                    MyFiles = enclosures,
                    SenderUserId = myTask.SenderUserId,
                    DestinationUserId = myTask.DestinationUserId,
                    MySubTasks = myTask.MySubTasks
                };

                string userId;
                List<User> users;
                if (User.Identity.IsAuthenticated)
                {
                    var userName = User.FindFirstValue(ClaimTypes.Name);
                    var currentUser = await _myUserService.GetCurrentUser(userName);
                    users = await _myUserService.GetUsers();
                    myTaskViewModel.DestinationUsers = from destinationUser in users select new SelectListItem { Text = destinationUser.UserNickName, Value = destinationUser.Id };
                }
                else
                {
                    return NotFound();
                }

                if (myTask != null) return View(myTaskViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditMyTask(MyTaskViewModel mtvm)
        {
           var destinationUser = await _myUserService.GetUserById(mtvm.DestinationUserId);
           
            if(await _myTaskService.EditMyTask(destinationUser, mtvm) == true)
            {
                return RedirectToAction("MyTasks");
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteMyEnclosure(int? id)
        {
            if (await _myTaskService.DeleteMyEnclosure(id) == true)
            {
                return new EmptyResult();
            }
            else
            {
                return NotFound();
            }

        }
        #endregion
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
            var typeOfContracts = await _contractsService.GetTypeOfContracts();
            IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
            IEnumerable<ArticleOfLaw> articleOfLaws = await _contractsService.GetArticleOfLaws();
            IEnumerable<MyContractStatus> myContractStatuses = await _contractsService.GetMyContractStatuses();
            myContractViewModel.TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
            myContractViewModel.ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };
            myContractViewModel.TypeOfContracts = from typeOfContract in typeOfContracts select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };
            myContractViewModel.MyContractStatuses = from statusOfContract in myContractStatuses select new SelectListItem { Text = statusOfContract.Name, Value = statusOfContract.Id.ToString() };
            return View(myContractViewModel);
        }

    [HttpPost]
        public async Task<IActionResult> CreateContract(MyContractViewModel mcvm)
        {

            // if (!ModelState.IsValid)
            // {
            //     return View(mcvm);
            // }

           if(await _contractsService.CreateContract(mcvm) == true)
            {
                return RedirectToAction("MyContracts");
            }
            else
            {
                return NotFound();
            }


        }

        public async Task<IActionResult> EditContract(int? id)
        {
            if (id != null)
            {
                Contract? contract = await _contractsService.GetContractByIdWithMyFiles(id.Value);
                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByContractId(id.Value);
                IEnumerable<Partner> myPartners = await _partnerService.GetPartners();
                var partners = from myPartner in myPartners select new SelectListItem { Text = myPartner.ShortName, Value = myPartner.Id.ToString() };
                IEnumerable<MyContractStatus> myContractStatuses = await _contractsService.GetMyContractStatuses();
                var contractStatuses = from statusOfContract in myContractStatuses select new SelectListItem { Text = statusOfContract.Name, Value = statusOfContract.Id.ToString() };
                var typeOfContracts = await _contractsService.GetTypeOfContracts();
                IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
                IEnumerable<ArticleOfLaw> articleOfLaws = await _contractsService.GetArticleOfLaws();

                MyContractViewModel myContractViewModel = new MyContractViewModel
                {
                    Id = contract.Id,
                    NumberOfContract = contract.NumberOfContract,
                    DateStart = contract.DateStart,
                    DateEnd = contract.DateEnd,
                    PartnerId = contract.PartnerId,
                    SubjectOfContract = contract.SubjectOfContract,
                    Cost = (decimal)contract.Cost,
                    MyPartners = partners,
                    MyContractStatuses = contractStatuses,
                    MyContractStatusId = contract.MyContractStatusId,
                    MyFiles = enclosures
                };

                myContractViewModel.TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
                myContractViewModel.ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };
                myContractViewModel.TypeOfContracts = from typeOfContract in typeOfContracts select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };


                if (contract != null) return View(myContractViewModel);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> EditContract(MyContractViewModel mcvm)
        {
            if (await _contractsService.EditContract(mcvm) == true)
            {
                return RedirectToAction("MyContracts");
            }
            else
            {
                return NotFound();
            }

        }


        [HttpPost]
        public async Task<IActionResult> DeleteContract(int? id)
        {
            if(await _contractsService.DeleteContract(id) == true)
            {
                return RedirectToAction("MyContracts");
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region [DocumentsConstructor]
        public IActionResult ConstructDocument()
        {
            return View();
        }

        public async Task<IActionResult> ConstructWorkContract()
        {

            DocumentConstructorViewModel dcvm = new DocumentConstructorViewModel();
            IEnumerable<Partner> myPartners = await _partnerService.GetPartners();
            IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
            IEnumerable<ArticleOfLaw> articleOfLaws =await _contractsService.GetArticleOfLaws();
            dcvm.MyPartners = from myPartner in myPartners select new SelectListItem { Text = myPartner.ShortName, Value = myPartner.Id.ToString() };
            dcvm.TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
            dcvm.ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };
            return View(dcvm);
        }

        [HttpPost]
        public async Task<IActionResult> ConstructWorkContract(DocumentConstructorViewModel dcvm)
        {
            Partner headOrganization = await _partnerService.GetOurOrganization();
            Partner? partner = await _partnerService.GetPartner(dcvm.PartnerId);
            DirectorType? directorType = await _partnerService.GetDirectorTypeById(partner.DirectorTypeId);
            DirectorType? headDirectorType = await _partnerService.GetDirectorTypeById(headOrganization.DirectorTypeId);

            string nameOfContract = "";
            string baseOfContract = "";
            if (dcvm.TypeOfStateRegId != 1)
            {
                nameOfContract = "Договор";
            }
            else
            {
                nameOfContract = "Контракт";
            }


            if (dcvm.ArticleOfLawId is not null)
            {
                if (dcvm.ArticleOfLawId == 1)
                    baseOfContract = "на основании пункта 8 части 1 статьи 93 Федерального Закона от 05.04.2013 № 44-ФЗ 'О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд',";
                else if (dcvm.ArticleOfLawId == 2)
                    baseOfContract = "на основании части 4 статьи 93 Федерального Закона от 05.04.2013 № 44-ФЗ 'О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд',";

            }
            else
            {
                if (dcvm.TypeOfStateRegId == 2)
                    baseOfContract = "на основании Федерального закона 'О закупках товаров, работ, услуг отдельными видами юридических лиц' от 18.07.2011 N 223-ФЗ,";
                else if (dcvm.TypeOfStateRegId == 3)
                {
                    baseOfContract = "";
                }
            }


            Dictionary<string, string> _replacePatterns = new Dictionary<string, string>()
            {
        { "NAMEOFCONTRACT", nameOfContract },
        { "BASEOFCONTRACT", baseOfContract },
        { "NUMBER", dcvm.NumberOfContract},
        { "DATESTART", ((DateTime)dcvm.DateStart).ToString("dd.MM.yyyy") },
        { "DATEEND", ((DateTime)dcvm.DateEnd).ToString("dd.MM.yyyy") },
        { "PARTNERFNAME", partner.Name },
        { "PARTNERDIR", directorType.Name },
        { "PARTNERDIRNAME", partner.DirectorName},
        { "SUBJECTOFCONTRACT", dcvm.SubjectOfContract},
        { "COST", Math.Truncate(Convert.ToDecimal(dcvm.Cost)).ToString() },
        { "COSTINWORDS", _documentConstructor.CostInWords(Convert.ToDecimal(dcvm.Cost)) },
        { "COIN", ((Convert.ToDecimal(dcvm.Cost)-Math.Truncate(Convert.ToDecimal(dcvm.Cost))).ToString()).Replace("0.","") },
        { "PARTNERSHORTNAME", partner.Name },
        { "ADDRESS", partner.Address},
        { "INN", partner.INN },
        { "KPP", partner.KPP },
        { "OGRN", partner.OGRN },
        { "ACCOUNT", partner.Account },
        { "CORRACCOUNT", partner.CorrespondentAccount},
        { "BANK", partner.Bank },
        { "BIK", partner.BIK },
        { "HEADFNAME", headOrganization.Name },
        { "HEADDIR", headDirectorType.Name },
        { "HEADDIRNAME", headOrganization.DirectorName},
        { "HEADSHORTNAME", headOrganization.Name },
        { "HEADADDRESS", headOrganization.Address},
        { "HEADINN", headOrganization.INN },
        { "HEADKPP", headOrganization.KPP },
        { "HEADOGRN", headOrganization.OGRN },
        { "HEADACCOUNT", headOrganization.Account },
        { "HEADCORRACCOUNT", headOrganization.CorrespondentAccount},
        { "HEADBANK", headOrganization.Bank },
        { "HEADBIK", headOrganization.BIK },
        { "PASSSERIES", partner.PassportSeries},
        { "PASSNUMBER", partner.PassportNumber },
        { "PASSDATEI", partner.PassportDateOfIssue.ToString() },
        { "DATEBIRTH", partner.PassportDateOfBirth.ToString() },
        { "PASSPLASEI", partner.PassportPlaseOfIssue },
        { "PASSDIVCODE", partner.PassportDivisionCode}
            };
            string fileName = String.Format(@"{0}", System.Guid.NewGuid());
            string appPath = _appEnvironment.WebRootPath;
            _documentConstructor.SetReplacePatterns(_replacePatterns);
            _documentConstructor.Replacing(appPath, "dogovorpodryada.docx", fileName);

            ConstructedDocumentViewModel cdvc = new ConstructedDocumentViewModel
            {
                NumberOfContract = dcvm.NumberOfContract,
                DateStart = dcvm.DateStart,
                DateEnd = dcvm.DateEnd,
                PartnerName = partner.Name,
                Cost = dcvm.Cost,
                SubjectOfContract = dcvm.SubjectOfContract,
                NameInServer = fileName + ".docx",
                Name = "dogovorpodryada.docx"
            };

            return RedirectToAction("ConstructedDocuments", cdvc);
        }

        public IActionResult ConstructedDocuments(ConstructedDocumentViewModel cdvc)
        {
            return View(cdvc);
        }

        [HttpPost]
        public async Task<string> ShadowConstructContract([FromBody] JsonDocument currentContractData)
        {

            CurrentContractData? dcvm = JsonSerializer.Deserialize<CurrentContractData>(currentContractData);
            Partner headOrganization = await _partnerService.GetOurOrganization();
            Partner? partner = await _partnerService.GetPartner(Convert.ToInt32(dcvm.PartnerId));
            DirectorType? directorType = await _partnerService.GetDirectorTypeById(partner.DirectorTypeId);
            DirectorType? headDirectorType = await _partnerService.GetDirectorTypeById(headOrganization.DirectorTypeId);


            Dictionary<string, string>? _replacePatterns = _documentConstructor.GetReplacePatterns(partner.PartnerTypeId, partner, headOrganization, dcvm, headDirectorType, directorType);

            string fileName = String.Format(@"{0}", System.Guid.NewGuid());
            string appPath = _appEnvironment.WebRootPath;
            _documentConstructor.SetReplacePatterns(_replacePatterns);
            DocumentTemplate docTemplate = new DocumentTemplate();
            docTemplate = await _templatesService.GetDocumentTemplateWithFilesById(Convert.ToInt32(dcvm.DocumentTemplateId));
            _documentConstructor.Replacing(appPath, docTemplate.TemplateFile.NameInServer, fileName);

            return ("\\Files\\Output\\" + fileName + ".docx");
        }

            public async Task<IActionResult> TemplatesDocuments()
        {
            return View(await _templatesService.GetAllTemplatesWitnToFAndToS());
        }

        public async Task<IActionResult> CreateTemplate()
        {
            TemplateViewModel templateViewModel = new TemplateViewModel();
            var typeOfContracts = await _contractsService.GetTypeOfContracts();
            var partnerTypes = await _partnerService.GetPartnerTypes();
            IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
            templateViewModel.TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
            templateViewModel.TypesOfContract = from typeOfContract in typeOfContracts select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };
            templateViewModel.PartnerTypes = from partnerType in partnerTypes select new SelectListItem { Text = partnerType.Name, Value = partnerType.Id.ToString() };

            return View(templateViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTemplate(TemplateViewModel tvm)
        {
            if(await _templatesService.CreateTemplate(tvm)==true)
            {
                return RedirectToAction("TemplatesDocuments");
            }
            else
            {
                return NotFound();
            }
        }

        public class TypeOfContract
        {
            public string id { get; set; }
        }
        private class DataOfTemplate
        {
            public string tCid { get; set; }
            public string pId { get; set; }
        }

        private class TemplateInformation
        {
            public string id { get; set; }
            public string nameOfTemplate { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> AddAccessibleTemplates([FromBody] JsonDocument dataOfTemplate)
        {
            DataOfTemplate dof = new DataOfTemplate();
            try
            { 
            dof = JsonSerializer.Deserialize<DataOfTemplate>(dataOfTemplate);
            }
            catch
            {
                int r = 0;
            }

            int typeOfContractId = Convert.ToInt32(dof.tCid);
            int partnerId = Convert.ToInt32(dof.pId);
            var partner = await _partnerService.GetPartner(partnerId);
            int partnerTypeId = partner.PartnerTypeId;
            IEnumerable<DocumentTemplate> templates = await _templatesService.GetTemplatesWithTypeOfContractAndPartnerType(typeOfContractId, partnerTypeId);
            List<TemplateInformation> templatesInfo = new List<TemplateInformation>();
            foreach (var template in templates)
            {
                TemplateInformation templ = new TemplateInformation();
                templ.id = template.Id.ToString();
                templ.nameOfTemplate = template.NameOfTemplate;
                templatesInfo.Add(templ);
            }            
            return Json(templatesInfo);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplate(int? id)
        {
            if (await _templatesService.DeleteTemplate(id) == true)
            {                
                return RedirectToAction("TemplatesDocuments");
            }
            else
            { 
                return NotFound();
            }
        }
        #endregion

        #region[subTasks]
        [HttpGet]
        public async Task<ActionResult> CreateSubTask(int id)
        {
            MySubTaskViewModel mySubTaskViewModel = new MySubTaskViewModel();
            var taskStatuses = await _myTaskService.GetMyTaskStatuses();
            var taskLevel = await _myTaskService.GetMyTaskLevels();
            mySubTaskViewModel.MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
            mySubTaskViewModel.MyTaskLevelImportances = from myTaskLevel in taskLevel select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };
            mySubTaskViewModel.MyTaskId = id;

            return PartialView(mySubTaskViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubTask(MySubTaskViewModel mtvm)
        {
            if(await _myTaskService.CreateSubTask(mtvm)==true)
            {
                return RedirectToAction("EditMyTask", new { id = mtvm.MyTaskId });
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditMySubTask(int? id)
        {
            if (id != null)
            {
                MySubTask? mySubTask = await _myTaskService.GetMySubTaskByIdWithFiles(id.Value);

                var taskStatuses = await _myTaskService.GetMyTaskStatusesToList();
                var taskLevels = await _myTaskService.GetMyTaskLevelsToList();

                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresBySubTaskId(id.Value);
                MySubTaskViewModel mySubTaskViewModel = new MySubTaskViewModel
                {
                    Id = mySubTask.Id,
                    MyTaskId = mySubTask.MyTaskId,
                    Name = mySubTask.Name,
                    Description = mySubTask.Description,
                    DateStart = mySubTask.DateStart,
                    DateEnd = mySubTask.DateEnd,
                    MyTaskStatusId = mySubTask.MyTaskStatusId,
                    MyTaskLevelImportanceId = mySubTask.MyTaskLevelImportanceId,
                    MyFiles = enclosures
                };

                mySubTaskViewModel.MyTaskStatuses = from myTaskStatus in taskStatuses select new SelectListItem { Text = myTaskStatus.Name, Value = myTaskStatus.Id.ToString() };
                mySubTaskViewModel.MyTaskLevelImportances = from myTaskLevel in taskLevels select new SelectListItem { Text = myTaskLevel.Name, Value = myTaskLevel.Id.ToString() };


                if (mySubTask!= null) return PartialView(mySubTaskViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditMySubTask(MySubTaskViewModel mtvm)
        {
        if(await _myTaskService.EditSubTask(mtvm)==true)
        {
            return RedirectToAction("EditMyTask", new { id = mtvm.MyTaskId });
        }
        else
        {
            return NotFound();
        }


        }

        [HttpPost]
        public async Task<IActionResult> DeleteMySubTask(int? id)
        {
            MySubTask currentSubTask = await _myTaskService.GetMySubTaskByIdWithFiles(id.Value);
            if (await _myTaskService.DeleteMySubTask(id, currentSubTask)==true)
            {
                return RedirectToAction("EditMyTask", new { id = currentSubTask.MyTaskId });
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region [Letters]
        public async Task<IActionResult> Letters()
        {
            return View(await _letterService.GetAllLettersEagerLoading());
        }

        public async Task<IActionResult> CreateLetter()
        {
            LetterViewModel letterViewModel = new LetterViewModel();
            var shippingMethods = await _letterService.GetAllShippingMethods();
            letterViewModel.ShippingMethods = from shippingMethod in shippingMethods select new SelectListItem { Text = shippingMethod.Name, Value = shippingMethod.Id.ToString() };
            var letterTypes = await _letterService.GetAllletterTypes();
            letterViewModel.LetterTypes = from letterType in letterTypes select new SelectListItem { Text = letterType.Name, Value = letterType.Id.ToString() };

            return View(letterViewModel);
        }




        [HttpPost]
        public async Task<IActionResult> CreateLetter(LetterViewModel letterViewModel)
        {
            if(await _letterService.CreateLetter(letterViewModel) == true)
            {
                return RedirectToAction("Letters");
            }
            else
            {
                return NotFound();
            }    
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLetter(int? id)
        {
            if (await _letterService.DeleteLetter(id)==true)
            {
                return RedirectToAction("Letters");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditLetter(int? id)
        {
            if (id != null)
            {
                Letter? letter = await _letterService.GetLetterById(id.Value);
                var shippingMethods = await _letterService.GetAllShippingMethods();
                var letterTypes = await _letterService.GetAllletterTypes();

                LetterViewModel letterViewModel = new LetterViewModel
                {
                    Number = letter.Number,
                    DepartureDate = letter.DepartureDate,
                    Name = letter.Name,
                    Destination = letter.Destination,
                    ShippingMethodId = letter.ShippingMethodId,
                    LetterTypeId = letter.LetterTypeId
                };

                letterViewModel.ShippingMethods = from shippingMethod in shippingMethods select new SelectListItem { Text = shippingMethod.Name, Value = shippingMethod.Id.ToString() };
                letterViewModel.LetterTypes = from letterType in letterTypes select new SelectListItem { Text = letterType.Name, Value = letterType.Id.ToString() };

                if (letter != null) return View(letterViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditLetter(Letter letter)
        {
            if(await _letterService.EditLetter(letter)==true)
            {
                return RedirectToAction("Letters");
            }
            else
            {
                return NotFound();
            }

        }
        #endregion

        #region[messages]

        #endregion
    }
}

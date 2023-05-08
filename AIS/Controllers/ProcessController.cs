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
using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using static AIS.Controllers.ProcessController;
using DocumentFormat.OpenXml.Presentation;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations;
using AIS.ViewModels.DocumentsViewModels;

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
        private ILetterService _letterService;
        private IEnclosureService _enclosureService;
        private IWebHostEnvironment _appEnvironment;
        private IConditionsService _conditionsService;
        private readonly UserManager<User> _userManager;
        IHubContext<AisHub> _hubContext;


        public ProcessController(ILogger<ProcessController> logger, 
                                 IWebHostEnvironment appEnvironment, 
                                 UserManager<User> userManager, 
                                 IPartnerService partnerService, 
                                 IEmployeeService employeeService, 
                                 IMyTaskService myTaskService, 
                                 IMyUsersService myUserService, 
                                 IContractsService contractsService, 
                                 ILetterService letterService, 
                                 IEnclosureService enclosureService, 
                                 IConditionsService conditionsService, 
                                 IHubContext<AisHub> hubContext)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _employeeService = employeeService;
            _partnerService = partnerService;
            _myTaskService = myTaskService;
            _myUserService = myUserService;
            _contractsService = contractsService;
            _letterService = letterService;
            _enclosureService = enclosureService;
            _hubContext = hubContext;
            _conditionsService = conditionsService;
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
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel evm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }



            if (await _employeeService.CreateEmployee(evm) == true)
            {
                return RedirectToAction("Employeers");
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
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

        public async Task<IActionResult> EditEmployee(int id)
        {
                IEnumerable<Partner> partners = await _partnerService.GetPartners();
                ViewBag.Partners = new SelectList(partners, "Id", "Name");
                Employee? employee = await _employeeService.GetEmployee(id);
                if (employee == null) return NotFound();
            EmployeeViewModel evm = new EmployeeViewModel()
            {
                Id = employee.Id,   
                Name = employee.Name,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
            };

            if (employee.PartnerId != null) evm.PartnerId = employee.PartnerId.Value;

            return View(evm);
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel evm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            if (await _employeeService.EditEmployee(evm) == true)
            {
                return RedirectToAction("Employeers");
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region [MyTasks]
        public async Task<IActionResult> MyTasks()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var currentUser = await _myUserService.GetCurrentUser(userName);
            if (User.IsInRole("admin"))
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
        public async Task<IActionResult> DeleteMyTask(int id)
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

        public async Task<IActionResult> EditMyTask(int id)
        {
                MyTask? myTask = await _myTaskService.GetMyTaskByIdEagerLoading(id);
                if(myTask == null) return NotFound();
                IEnumerable<MyTaskStatus> myTaskStatuses = await _myTaskService.GetMyTaskStatuses();
                ViewBag.MyTaskStatuses = new SelectList(myTaskStatuses, "Id", "Name");
                IEnumerable<LevelImportance> myTaskLevels = await _myTaskService.GetMyTaskLevels();
                ViewBag.MyTaskLevels = new SelectList(myTaskLevels, "Id", "Name");
                IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByTaskId(id);
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

             return View(myTaskViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> EditMyTask(MyTaskViewModel mtvm)
        {
            var destinationUser = await _myUserService.GetUserById(mtvm.DestinationUserId);

            if (await _myTaskService.EditMyTask(destinationUser, mtvm) == true)
            {
                return RedirectToAction("MyTasks");
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteMyEnclosure(int id)
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

            if (await _contractsService.CreateContract(mcvm, typeOfContractId) == true)
            {     
                return RedirectToAction("MyContracts");
            }
            else
            {
                return NotFound();
            }
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
        public async Task<IActionResult> DeleteContract(int id)
        {
            if (await _contractsService.DeleteContract(id) == true)
            {
                return RedirectToAction("MyContracts");
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
            if (await _myTaskService.CreateSubTask(mtvm) == true)
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


                if (mySubTask != null) return PartialView(mySubTaskViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditMySubTask(MySubTaskViewModel mtvm)
        {
            if (await _myTaskService.EditSubTask(mtvm) == true)
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
            if (await _myTaskService.DeleteMySubTask(id, currentSubTask) == true)
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
            if (await _letterService.CreateLetter(letterViewModel) == true)
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
            if (await _letterService.DeleteLetter(id) == true)
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
            if (await _letterService.EditLetter(letter) == true)
            {
                return RedirectToAction("Letters");
            }
            else
            {
                return NotFound();
            }

        }
        #endregion

        #region[DocumentGenerator]
        public async Task<IActionResult> RootTemplates()
        {
            return View(await _conditionsService.GetRootTemplates());
        }

        public async Task<IActionResult> CreateRootTemplate()
        {          
            RootTemplateViewModel RootTemplateViewModel = new RootTemplateViewModel();
            var documentTypes = await _contractsService.GetTypesOfDocument();
            RootTemplateViewModel.TypesOfDocument = from documentType in documentTypes select new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString() };

            return View(RootTemplateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRootTemplate(RootTemplateViewModel cctvm)
        {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        if (await _conditionsService.CreateRootTemplate(cctvm) == true)
            {
                return RedirectToAction("RootTemplates");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRootTemplate(int id)
        {
                var documentTypes = await _contractsService.GetTypesOfDocument();

                RootTemplate? rootTemplate = await _conditionsService.GetRootTemplateWithDocumentTemplatesById(id);                
                if(rootTemplate == null) return NotFound();
    
                RootTemplateViewModel rootTemplateViewModel = new RootTemplateViewModel
                {
                    Id = rootTemplate.Id,
                    Name = rootTemplate.Name,
                    Description = rootTemplate.Description,
                    DocumentTemplates = rootTemplate.DocumentTemplates,
                    TypeOfDocumentId = rootTemplate.TypeOfDocumentId,
                };
                rootTemplateViewModel.TypesOfDocument = from documentType in documentTypes select new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString() };

                return View(rootTemplateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRootTemplate(RootTemplateViewModel cctvm)
        {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        if (await _conditionsService.EditRootTemplate(cctvm) == true)
            {
                return RedirectToAction("RootTemplates");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRootTemplate(int id)
        {
            if (await _conditionsService.DeleteRootTemplate(id) == true)
            {
                return RedirectToAction("RootTemplates");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DocumentTemplates()
        {
            return View(await _conditionsService.GetDocumentTemplates());
        }

        public async Task<IActionResult> CreateDocumentTemplate(int id)
        {
            DocumentTemplateViewModel DocumentTemplateViewModel = new DocumentTemplateViewModel();
            DocumentTemplateViewModel.RootTemplateId = id;
            RootTemplate rootTemplate = await _conditionsService.GetRootTemplateById(id);
            DocumentTemplateViewModel.TypeOfDocumentId = rootTemplate.TypeOfDocumentId;
            var typesOfContract = await _contractsService.GetTypeOfContracts();
            DocumentTemplateViewModel.TypesOfContract = from typeOfContract in typesOfContract select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };
            return View(DocumentTemplateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocumentTemplate(DocumentTemplateViewModel ctvm)
        {
            if(ctvm.TypeOfDocumentId == 1)
            { 
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            }
            else
            {
                if (ctvm.Name == null) return NotFound();
                if (ctvm.Description == null) return NotFound();
            }

            if (await _conditionsService.CreateDocumentTemplate(ctvm) == true)
            {
                return RedirectToAction("EditRootTemplate", new { id = ctvm.RootTemplateId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditDocumentTemplate(int id)
        {       
                DocumentTemplate? DocumentTemplate = await _conditionsService.GetDocumentTemplateWithConditionsById(id);
                if (DocumentTemplate == null) return NotFound();
    
                var typesOfContract = await _contractsService.GetTypeOfContracts();
                var typesOfStateReg = await _contractsService.GetTypeOfStateRegs();
                var conditions = DocumentTemplate.Conditions;
                conditions = conditions.OrderBy(x => x.NumberInDocument).ToList<Core.Condition>();
                DocumentTemplateViewModel DocumentTemplateViewModel = new DocumentTemplateViewModel
                {
                    Id = DocumentTemplate.Id,
                    Name = DocumentTemplate.Name,
                    Description = DocumentTemplate.Description,
                    Conditions = conditions
                };
                if (DocumentTemplate.TypeOfContractId != null) DocumentTemplateViewModel.TypeOfContractId = DocumentTemplate.TypeOfContractId.Value;

                DocumentTemplateViewModel.RootTemplateId = DocumentTemplate.RootTemplateId;
                DocumentTemplateViewModel.TypesOfContract = from typeOfContract in typesOfContract select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };
                DocumentTemplateViewModel.TypesOfStateReg = from typeOfStateReg in typesOfStateReg select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };


                return View(DocumentTemplateViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> EditDocumentTemplate(DocumentTemplateViewModel ctvm)
        {
            if (ctvm.TypeOfDocumentId != 1)
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
            }
            else
            {
                if (ctvm.Name == null) return NotFound();
                if (ctvm.Description == null) return NotFound();
            }

            if (await _conditionsService.EditDocumentTemplate(ctvm) == true)
            {
                return RedirectToAction("EditRootTemplate", new { id = ctvm.RootTemplateId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocumentTemplate(int id)
        {
            DocumentTemplate documentTemplate = await _conditionsService.GetDocumentTemplateById(id);
            int RootTemplateId = documentTemplate.RootTemplateId;
            if (await _conditionsService.DeleteDocumentTemplate(id) == true)
            {
                return RedirectToAction("EditRootTemplate", new { id = RootTemplateId });
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateCondition(int id)
        {
                ConditionViewModel conditionViewModel = new ConditionViewModel();
                DocumentTemplate? documentTemplate = await _conditionsService.GetDocumentTemplateWithRootTemplateById(id);
                if (documentTemplate == null) return NotFound();
                int typeOfDocumentId = documentTemplate.RootTemplate.TypeOfDocumentId;
                conditionViewModel.DocumentTemplateId = id;
                conditionViewModel.TypeOfDocumentId = typeOfDocumentId;
                var typesOfStateReg = await _contractsService.GetTypeOfStateRegs();
                conditionViewModel.TypesOfStateReg = from typeOfStateReg in typesOfStateReg select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
                return View(conditionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCondition(ConditionViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            if (await _conditionsService.CreateCondition(cvm) == true)
            {
                return RedirectToAction("EditDocumentTemplate", new {id = cvm.DocumentTemplateId});
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCondition(int id)
        {
                ConditionViewModel conditionViewModel = new ConditionViewModel();
                Core.Condition? condition = await _conditionsService.GetCondition(id);
                if(condition == null) return NotFound();
                DocumentTemplate? documentTemplate = await _conditionsService.GetDocumentTemplateWithRootTemplateById(condition.DocumentTemplateId);
                if (documentTemplate == null) return NotFound();
                conditionViewModel.Id = condition.Id;
                if(condition.TypeOfStateRegId != null) conditionViewModel.TypeOfStateRegId = condition.TypeOfStateRegId.Value;
                conditionViewModel.Title = condition.Title;
                conditionViewModel.Name = condition.Name;
                conditionViewModel.DocumentTemplateId = condition.DocumentTemplateId;
                int typeOfDocumentId = documentTemplate.RootTemplate.TypeOfDocumentId;
                conditionViewModel.TypeOfDocumentId = typeOfDocumentId;
                conditionViewModel.NumLevelReference = condition.NumLevelReference;
                conditionViewModel.NumId = condition.NumId;
                conditionViewModel.Justification = condition.Justification;
                conditionViewModel.SubConditions = condition.SubConditions;
                var typesOfStateReg = await _contractsService.GetTypeOfStateRegs();
                conditionViewModel.TypesOfStateReg = from typeOfStateReg in typesOfStateReg select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
                return View(conditionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCondition(ConditionViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            if (await _conditionsService.EditCondition(cvm) == true)
            {
                return RedirectToAction("EditDocumentTemplate", new { id = cvm.DocumentTemplateId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LiftUpCondition(int id)
        {
            Core.Condition? condition = await _conditionsService.GetCondition(id);
            if (condition == null) return NotFound();
            if(condition.NumberInDocument != 1)
            { 
            Core.Condition lowerDownCondition = await _conditionsService.GetConditionByNumberInDocument(condition.NumberInDocument-1);
                condition.NumberInDocument = lowerDownCondition.NumberInDocument;
                lowerDownCondition.NumberInDocument = lowerDownCondition.NumberInDocument + 1;
                _conditionsService.SaveCondition(condition);
                _conditionsService.SaveCondition(lowerDownCondition);
            }
            return RedirectToAction("EditDocumentTemplate", new { id = condition.DocumentTemplateId });

        }

        [HttpPost]
        public async Task<IActionResult> LowerDownCondition(int id)
        {
            Core.Condition? condition = await _conditionsService.GetCondition(id);
            if (condition == null) return NotFound();
            Core.Condition liftUpCondition = await _conditionsService.GetConditionByNumberInDocument(condition.NumberInDocument + 1);
            if(liftUpCondition != null)
            {
                condition.NumberInDocument = liftUpCondition.NumberInDocument;
                liftUpCondition.NumberInDocument = liftUpCondition.NumberInDocument - 1;
                _conditionsService.SaveCondition(condition);
                _conditionsService.SaveCondition(liftUpCondition);

            }
            return RedirectToAction("EditDocumentTemplate", new { id = condition.DocumentTemplateId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCondition(int id)
        {
            Core.Condition? condition = await _conditionsService.GetCondition(id);
            if (condition == null) return NotFound();
            int DocumentTemplateId = condition.DocumentTemplateId;
            if (await _conditionsService.DeleteCondition(id) == true)
            {
                return RedirectToAction("EditDocumentTemplate", new { id = DocumentTemplateId });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet]
        public async Task<IActionResult> CreateSubCondition(int id)
        {
                SubConditionViewModel subConditionViewModel = new SubConditionViewModel();
                subConditionViewModel.ConditionId = id;
                return View(subConditionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubCondition(SubConditionViewModel scvm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (await _conditionsService.CreateSubCondition(scvm) == true)
            {
                return RedirectToAction("EditCondition", new { id = scvm.ConditionId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSubCondition(int id)
        {
                SubConditionViewModel subConditionViewModel = new SubConditionViewModel();
                SubCondition subCondition = await _conditionsService.GetSubCondition(id);
                if (subCondition == null) return NotFound();
                subConditionViewModel.Id = subCondition.Id;
                subConditionViewModel.Name = subCondition.Name;
                subConditionViewModel.Text = subCondition.Text;
                subConditionViewModel.ConditionId = subCondition.ConditionId;
                subConditionViewModel.NumLevelReference = subCondition.NumLevelReference;
                subConditionViewModel.NumId = subCondition.NumId;
                subConditionViewModel.Justification = subCondition.Justification;
                subConditionViewModel.SubConditionParagraphs = subCondition.SubConditionParagraphs;
                return View(subConditionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubCondition(SubConditionViewModel scvm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (await _conditionsService.EditSubCondition(scvm) == true)
            {
                return RedirectToAction("EditCondition", new { id = scvm.ConditionId });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSubCondition(int id)
        {
            SubCondition subCondition = await _conditionsService.GetSubCondition(id);
            if (subCondition == null) return NotFound();
            int conditionId = subCondition.ConditionId;
            if (await _conditionsService.DeleteSubCondition(id) == true)
            {
                return RedirectToAction("EditCondition", new { id = conditionId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateSubConditionParagraph(int id)
        {
                SubConditionParagraphViewModel subConditionParagraphViewModel = new SubConditionParagraphViewModel();
                subConditionParagraphViewModel.SubConditionId = id;
                return View(subConditionParagraphViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubConditionParagraph(SubConditionParagraphViewModel scpvm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            if (await _conditionsService.CreateSubConditionParagraph(scpvm) == true)
            {
                return RedirectToAction("EditSubCondition", new { id = scpvm.SubConditionId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSubConditionParagraph(int id)
        {
                SubConditionParagraphViewModel subConditionParagraphViewModel = new SubConditionParagraphViewModel();
                SubConditionParagraph? subConditionParagraph = await _conditionsService.GetSubConditionParagraph(id);
                if (subConditionParagraph == null) return NotFound();
                subConditionParagraphViewModel.Id = subConditionParagraph.Id;
                subConditionParagraphViewModel.Text = subConditionParagraph.Text;
                subConditionParagraphViewModel.NumLevelReference = subConditionParagraph.NumLevelReference;
                subConditionParagraphViewModel.NumId = subConditionParagraph.NumId;
                subConditionParagraphViewModel.Justification = subConditionParagraph.Justification;
                subConditionParagraphViewModel.SubConditionId = subConditionParagraph.SubConditionId;
                return View(subConditionParagraphViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubConditionParagraph(SubConditionParagraphViewModel scpvm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            if (await _conditionsService.EditSubConditionParagraph(scpvm) == true)
            {
                return RedirectToAction("EditSubCondition", new { id = scpvm.SubConditionId });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSubConditionParagraph(int id)
        {
            SubConditionParagraph? subConditionParagraph = await _conditionsService.GetSubConditionParagraph(id);
            if (subConditionParagraph == null) return NotFound();
            int subConditionId = subConditionParagraph.SubConditionId;
            if (await _conditionsService.DeleteSubConditionParagraph(id) == true)
            {
                return RedirectToAction("EditSubCondition", new { id = subConditionId });
            }
            else
            {
                return NotFound();
            }
        }

        public class JsonContractData
        {
            public string NumberOfContract { get; set; }
            public string DateStart { get; set; }
            public string DateEnd { get; set; }
            public string PartnerId { get; set; }
            public string SubjectOfContract { get; set; }
            public string Cost { get; set; }
            public string TypeOfStateRegId { get; set; }
            public string ArticleOfLawId { get; set; }
            public string DocumentTemplateId { get; set; }
            public string IsCustomer { get; set; }
            public string PlaceOfContract { get; set; }
        }

        [HttpPost]
        public async Task<string> ShadowConstructDocument([FromBody] JsonContractData jsonContractData)
        {
            bool isCustomer;
            Dictionary <string, string> replacementDictionary = new Dictionary<string, string>();
            DocumentTemplate DocumentTemplate = await _conditionsService.GetDocumentTemplateWithTypeOfContractById(Convert.ToInt32(jsonContractData.DocumentTemplateId));

            RootTemplate RootTemplate = await _conditionsService.GetRootTemplateWithDocumentTemplatesById(DocumentTemplate.RootTemplateId);
            int typeOfDocumentId = RootTemplate.TypeOfDocumentId;
            if (jsonContractData.IsCustomer == "true") isCustomer = true;
            else isCustomer = false;
            if (jsonContractData.ArticleOfLawId == "") jsonContractData.ArticleOfLawId = "0";
            DocumentModel contract = new DocumentModel();
            contract.TypeOfDocumentId = typeOfDocumentId;

            List<Core.Condition> conditions = new List<Core.Condition>();

            DocumentTemplate documentTemplate = _conditionsService.GetDocumentTemplateEagerLoadingById(Convert.ToInt32(jsonContractData.DocumentTemplateId));
            int typeOfContract = documentTemplate.TypeOfContractId.Value;
            foreach (var condition in DocumentTemplate.Conditions)
            {
                //Добавляем все общие условия
                if (condition.TypeOfStateRegId == 4)
                {
                    conditions.Add(condition);
                }
                //Если 44-ФЗ добавляем специфические условия для этого типа регулирования               
                if (Convert.ToInt32(jsonContractData.TypeOfStateRegId) == 1)
                {
                    if (condition.TypeOfStateRegId == 1)
                    {
                        conditions.Add(condition);
                    }
                }
            }

            conditions = conditions.OrderBy(x => x.NumberInDocument).ToList<Core.Condition>();
            contract.Conditions = conditions;

            string contractType = "";
            string contractName = "";
            string baseOfContract = "";
            string paragraphBaseOfContract = "";
            string executor = "";

            //Получаем тип договора
            switch (typeOfContract)
            {
                case 1:
                    contractType = "подряда";
                    executor = "Подрядчик";
                    break;
                case 2:
                    contractType = "оказания услуг";
                    executor = "Исполнитель";
                    break;
                case 3:
                    contractType = "поставки";
                    executor = "Поставщик";
                    break;
                case 4:
                    contractType = "аренды";
                    executor = "Арендатор";
                    break;
            }

            //Получаем пункт основания заключения контракта (для 44-ФЗ)
            if (Convert.ToInt32(jsonContractData.TypeOfStateRegId) == 1)
            {
                switch (Convert.ToInt32(jsonContractData.ArticleOfLawId))
                {
                    case 1:
                        paragraphBaseOfContract = "п. 4 ст. 93 ";
                        break;
                    case 2:
                        paragraphBaseOfContract = "п. 8 ст. 93 ";
                        break;
                }
            }

            //Получаем фактическое наименование контракта и основание заключения
            switch (Convert.ToInt32(jsonContractData.TypeOfStateRegId))
            {
                case 1:
                    contractName = "Контракт";
                    baseOfContract = "на основании " + paragraphBaseOfContract + "федерального закона \"О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд\" от 05.04.2013 N 44-ФЗ,";
                    break;
                case 2:
                    contractName = "Договор";
                    baseOfContract = "на основании федерального закона \"О закупках товаров, работ, услуг отдельными видами юридических лиц\" от 18.07.2011 N 223-ФЗ,";
                    break;
                case 3:
                    contractName = "Договор";
                    break;

            }


            

            Partner contragent = await _partnerService.GetPartner(Convert.ToInt32(jsonContractData.PartnerId));
            Partner mainOrganization = await _partnerService.GetOurOrganization();



            contract = _contractsService.SetContractRequisites(contract, isCustomer, mainOrganization, contragent);

            replacementDictionary.Add("договор", contractName);
            replacementDictionary.Add("contractType", contractType);
            if(isCustomer == true)
            {
                replacementDictionary.Add("customerName", mainOrganization.Name);
                replacementDictionary.Add("executorName", contragent.Name);
                replacementDictionary.Add("customerShortName", mainOrganization.ShortName);
                replacementDictionary.Add("executorShortName", contragent.ShortName);
                replacementDictionary.Add("customerDirectorNameR", mainOrganization.DirectorNameR);
                replacementDictionary.Add("executorDirectorNameR", contragent.DirectorNameR);
                replacementDictionary.Add("customerDirectorTypeNameR", mainOrganization.DirectorType.NameR);
                replacementDictionary.Add("executorDirectorTypeNameR", contragent.DirectorType.NameR);
            }
            else
            {
                replacementDictionary.Add("customerName", contragent.Name);
                replacementDictionary.Add("executorName", mainOrganization.Name);
                replacementDictionary.Add("customerDirectorNameR", contragent.DirectorNameR);
                replacementDictionary.Add("executorDirectorNameR", mainOrganization.DirectorNameR);
                replacementDictionary.Add("customerDirectorTypeNameR", contragent.DirectorType.NameR);
                replacementDictionary.Add("executorDirectorTypeNameR", mainOrganization.DirectorType.NameR);
            }

            replacementDictionary.Add("place", jsonContractData.PlaceOfContract);

            replacementDictionary.Add("baseOfContract", baseOfContract);
            replacementDictionary.Add("subjectOfContract", jsonContractData.SubjectOfContract);
            replacementDictionary.Add("dateEnd", jsonContractData.DateEnd);
            replacementDictionary.Add("executor", executor);
            replacementDictionary.Add("cost", jsonContractData.Cost);

            contract.ReplacementDictionary = replacementDictionary;
            
            string fileName = Guid.NewGuid().ToString();
            fileName = fileName + ".docx";
            string path = _appEnvironment.WebRootPath + "\\files\\Output\\" + fileName;
            new DocumentGenerator().CreateContract(path, contract);

            return path;
        }

        [HttpGet]
        public async Task<IActionResult> CreateCancellationOfCourtOrder()
        {
                CancellationOfCourtOrderViewModel ccovm = new CancellationOfCourtOrderViewModel();
                var courts = await _partnerService.GetPartnersByPartnerCategoryId(2);
                ccovm.Courts = from court in courts select new SelectListItem { Text = court.Name, Value = court.Id.ToString() };
                var  partners = await _partnerService.GetPartners();
                ccovm.MyPartners = from partner in partners select new SelectListItem { Text = partner.Name, Value = partner.Id.ToString() };

            return View(ccovm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCancellationOfCourtOrder(CancellationOfCourtOrderViewModel ccovm)
        {
            return RedirectToAction("MyTasks");
        }

        public IActionResult Templates()
        {
            return View();
        }
        #endregion

        #region[pref]
        public IActionResult Pref()
        {
            return View();
        }
        #endregion

    }
}

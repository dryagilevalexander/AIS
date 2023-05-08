﻿using AIS.Models;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using AIS.ViewModels.ProcessViewModels;

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

        #region[pref]
        public IActionResult Pref()
        {
            return View();
        }
        #endregion

    }
}

using AIS.Models;
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

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class DocumentsController : Controller
    {
        private readonly ILogger<ProcessController> _logger;
        private IPartnerService _partnerService;
        private IContractsService _contractsService;
        private IWebHostEnvironment _appEnvironment;
        private IConditionsService _conditionsService;


        public DocumentsController(ILogger<ProcessController> logger, 
                                 IWebHostEnvironment appEnvironment, 
                                 IContractsService contractsService, 
                                 IConditionsService conditionsService)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _contractsService = contractsService;
            _conditionsService = conditionsService;
        }

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

           await _conditionsService.CreateRootTemplate(cctvm);
           return RedirectToAction("RootTemplates");
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

            await _conditionsService.EditRootTemplate(cctvm);
            return RedirectToAction("RootTemplates");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRootTemplate(int id)
        {
            await _conditionsService.DeleteRootTemplate(id);
            return RedirectToAction("RootTemplates");
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

            await _conditionsService.CreateDocumentTemplate(ctvm);
            return RedirectToAction("EditRootTemplate", new { id = ctvm.RootTemplateId });
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

            await _conditionsService.EditDocumentTemplate(ctvm);
            return RedirectToAction("EditRootTemplate", new { id = ctvm.RootTemplateId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocumentTemplate(int id)
        {
            DocumentTemplate documentTemplate = await _conditionsService.GetDocumentTemplateById(id);
            int RootTemplateId = documentTemplate.RootTemplateId;
            await _conditionsService.DeleteDocumentTemplate(id);
            return RedirectToAction("EditRootTemplate", new { id = RootTemplateId });
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

            await _conditionsService.CreateCondition(cvm);
            return RedirectToAction("EditDocumentTemplate", new {id = cvm.DocumentTemplateId});
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

            await _conditionsService.EditCondition(cvm);
            return RedirectToAction("EditDocumentTemplate", new { id = cvm.DocumentTemplateId });
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
            await _conditionsService.DeleteCondition(id);
            return RedirectToAction("EditDocumentTemplate", new { id = DocumentTemplateId });
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

            await _conditionsService.CreateSubCondition(scvm);
            return RedirectToAction("EditCondition", new { id = scvm.ConditionId });
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

            await _conditionsService.EditSubCondition(scvm);
            return RedirectToAction("EditCondition", new { id = scvm.ConditionId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSubCondition(int id)
        {
            SubCondition subCondition = await _conditionsService.GetSubCondition(id);
            if (subCondition == null) return NotFound();
            int conditionId = subCondition.ConditionId;
            await _conditionsService.DeleteSubCondition(id);
            return RedirectToAction("EditCondition", new { id = conditionId });
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

            await _conditionsService.CreateSubConditionParagraph(scpvm);
            return RedirectToAction("EditSubCondition", new { id = scpvm.SubConditionId });
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

            await _conditionsService.EditSubConditionParagraph(scpvm);
            return RedirectToAction("EditSubCondition", new { id = scpvm.SubConditionId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSubConditionParagraph(int id)
        {
            SubConditionParagraph subConditionParagraph = await _conditionsService.GetSubConditionParagraph(id);
            int subConditionId = subConditionParagraph.SubConditionId;
            await _conditionsService.DeleteSubConditionParagraph(id);
            return RedirectToAction("EditSubCondition", new { id = subConditionId });
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
            Partner mainOrganization = await _partnerService.GetMainOrganization();



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

    }
}

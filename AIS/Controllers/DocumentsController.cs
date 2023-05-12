using AIS.Models;
using Infrastructure;
using Infrastructure.Models;
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
using AIS.ErrorManager;
using System.Net;

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
            CreateRootTemplateViewModel model = new CreateRootTemplateViewModel();
            await model.Fill(_contractsService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRootTemplate(CreateRootTemplateViewModel model)
        {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

           await _conditionsService.CreateRootTemplate(model);
           return RedirectToAction("RootTemplates");
        }

        [HttpGet]
        public async Task<IActionResult> EditRootTemplate(int id)
        {
                EditRootTemplateViewModel model = new EditRootTemplateViewModel();
                await model.Fill(id, _conditionsService, _contractsService);
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRootTemplate(EditRootTemplateViewModel model)
        {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

            await _conditionsService.EditRootTemplate(model);
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
            CreateDocumentTemplateViewModel model = new CreateDocumentTemplateViewModel();
            await model.Fill(id, _conditionsService, _contractsService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocumentTemplate(CreateDocumentTemplateViewModel model)
        {
            if(model.TypeOfDocumentId == 1)
            { 
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            }
            else
            {
                if (model.Name == null) return NotFound();
                if (model.Description == null) return NotFound();
            }

            await _conditionsService.CreateDocumentTemplate(model);
            return RedirectToAction("EditRootTemplate", new { id = model.RootTemplateId });
        }

        [HttpGet]
        public async Task<IActionResult> EditDocumentTemplate(int id)
        {
            EditDocumentTemplateViewModel model = new EditDocumentTemplateViewModel();
            await model.Fill(id, _conditionsService, _contractsService);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditDocumentTemplate(EditDocumentTemplateViewModel model)
        {
            if (model.TypeOfDocumentId != 1)
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
            }
            else
            {
                if (model.Name == null) return NotFound();
                if (model.Description == null) return NotFound();
            }

            await _conditionsService.EditDocumentTemplate(model);
            return RedirectToAction("EditRootTemplate", new { id = model.RootTemplateId });
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
            CreateConditionViewModel model = new CreateConditionViewModel();
            await model.Fill(id, _conditionsService, _contractsService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCondition(CreateConditionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _conditionsService.CreateCondition(model);
            return RedirectToAction("EditDocumentTemplate", new {id = model.DocumentTemplateId});
        }

        [HttpGet]
        public async Task<IActionResult> EditCondition(int id)
        {
                EditConditionViewModel model = new EditConditionViewModel();
                await model.Fill(id, _conditionsService, _contractsService);
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCondition(EditConditionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _conditionsService.EditCondition(model);
            return RedirectToAction("EditDocumentTemplate", new { id = model.DocumentTemplateId });
        }

        [HttpPost]
        public async Task<IActionResult> LiftUpCondition(int id)
        {
            Infrastructure.Models.Condition condition = await _conditionsService.GetCondition(id);
            if (condition.NumberInDocument != 1)
            { 
            Infrastructure.Models.Condition lowerDownCondition = await _conditionsService.GetConditionByNumberInDocument(condition.NumberInDocument-1);
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
            Infrastructure.Models.Condition? condition = await _conditionsService.GetCondition(id);
            Infrastructure.Models.Condition liftUpCondition = await _conditionsService.GetConditionByNumberInDocument(condition.NumberInDocument + 1);
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
            Infrastructure.Models.Condition? condition = await _conditionsService.GetCondition(id);
            if (condition == null) return NotFound();
            int DocumentTemplateId = condition.DocumentTemplateId;
            await _conditionsService.DeleteCondition(id);
            return RedirectToAction("EditDocumentTemplate", new { id = DocumentTemplateId });
        }


        [HttpGet]
        public async Task<IActionResult> CreateSubCondition(int id)
        {
                CreateSubConditionViewModel model = new CreateSubConditionViewModel();
                model.ConditionId = id;
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubCondition(CreateSubConditionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _conditionsService.CreateSubCondition(model);
            return RedirectToAction("EditCondition", new { id = model.ConditionId });
        }

        [HttpGet]
        public async Task<IActionResult> EditSubCondition(int id)
        {
            EditSubConditionViewModel model = new EditSubConditionViewModel();
            model.Fill(id, _conditionsService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubCondition(EditSubConditionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _conditionsService.EditSubCondition(model);
            return RedirectToAction("EditCondition", new { id = model.ConditionId });
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
                CreateSubConditionParagraphViewModel model = new CreateSubConditionParagraphViewModel();
                model.SubConditionId = id;
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubConditionParagraph(CreateSubConditionParagraphViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _conditionsService.CreateSubConditionParagraph(model);
            return RedirectToAction("EditSubCondition", new { id = model.SubConditionId });
        }

        [HttpGet]
        public async Task<IActionResult> EditSubConditionParagraph(int id)
        {
            EditSubConditionParagraphViewModel model = new EditSubConditionParagraphViewModel();
            model.Fill(id, _conditionsService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubConditionParagraph(EditSubConditionParagraphViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _conditionsService.EditSubConditionParagraph(model);
            return RedirectToAction("EditSubCondition", new { id = model.SubConditionId });
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

            List<Infrastructure.Models.Condition> conditions = new List<Infrastructure.Models.Condition>();

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

            conditions = conditions.OrderBy(x => x.NumberInDocument).ToList<Infrastructure.Models.Condition>();
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

using AIS.ErrorManager;
using AIS.ViewModels.DocumentsViewModels;
using Core;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIS.ErrorManager;
using System.Net;


namespace AIS.Services
{
    public class ConditionsService: IConditionsService
    {
        private CoreContext db;
        IWebHostEnvironment _appEnvironment;
        public ConditionsService(CoreContext coreContext, IWebHostEnvironment appEnvironment)
        {
            db = coreContext;
            _appEnvironment = appEnvironment;
        }

        public async Task<List<RootTemplate>> GetRootTemplates()
        {
            return await db.RootTemplates.ToListAsync();
        }

        public async Task<RootTemplate> GetRootTemplateById(int id)
        {
            return await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<RootTemplate> GetRootTemplateWithDocumentTemplatesById(int id)
        {
            return await db.RootTemplates.Include(p => p.DocumentTemplates).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<DocumentTemplate>> GetDocumentTemplates()
        {
            return await db.DocumentTemplates.ToListAsync();
        }

        //Метод получения шаблона контракта с стандартными условиями для всех типов регулирования
        public DocumentTemplate GetDocumentTemplateEagerLoadingById(int id)
        {
            return db.DocumentTemplates.Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(c => c.SubConditionParagraphs).FirstOrDefault(p => p.Id == id);
        }

        public async Task CreateRootTemplate(RootTemplateViewModel cctvm)
        {
            try
            {
                RootTemplate RootTemplate = new RootTemplate();

                RootTemplate.Name = cctvm.Name;
                RootTemplate.Description = cctvm.Description;
                RootTemplate.TypeOfDocumentId = cctvm.TypeOfDocumentId;

                db.RootTemplates.Add(RootTemplate);
                await db.SaveChangesAsync();
            }

            catch
            {
                throw new AisException("Не удалось создать шаблон", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditRootTemplate(RootTemplateViewModel cctvm)
        {
            try
            {
                RootTemplate RootTemplate = await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == cctvm.Id);
                RootTemplate.Name = cctvm.Name;
                RootTemplate.Description = cctvm.Description;
                RootTemplate.TypeOfDocumentId = cctvm.TypeOfDocumentId;

                db.RootTemplates.Update(RootTemplate);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось открыть шаблон для редактирования", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteRootTemplate(int id)
        {
            try
            {
                RootTemplate RootTemplate = await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == id);
                db.RootTemplates.Remove(RootTemplate);
                await db.SaveChangesAsync();
            }   
            catch
            {
                throw new AisException("Не удалось удалить шаблон", HttpStatusCode.BadRequest);
            }
        }


        public async Task CreateDocumentTemplate(DocumentTemplateViewModel ctvm)
        {

            try
            {
                DocumentTemplate documentTemplate = new DocumentTemplate
                {
                    Name = ctvm.Name,
                    Description = ctvm.Description,
                    RootTemplateId = ctvm.RootTemplateId,
                    TypeOfDocumentId = ctvm.TypeOfDocumentId
                };
                if (ctvm.TypeOfContractId != null) documentTemplate.TypeOfContractId = ctvm.TypeOfContractId;

                db.DocumentTemplates.Add(documentTemplate);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать шаблон", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditDocumentTemplate(DocumentTemplateViewModel ctvm)
        {
            try
            {
                DocumentTemplate DocumentTemplate = await db.DocumentTemplates.FirstOrDefaultAsync(p => p.Id == ctvm.Id);
                DocumentTemplate.Name = ctvm.Name;
                DocumentTemplate.Description = ctvm.Description;
                DocumentTemplate.TypeOfContractId = ctvm.TypeOfContractId;

                db.DocumentTemplates.Update(DocumentTemplate);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось открыть шаблон для редактирования", HttpStatusCode.BadRequest);
            }
        }

        public async Task CreateCondition(ConditionViewModel cvm)
        {
            try
            {
                Condition condition = new Condition();

                    condition.Title = cvm.Title;
                    condition.Name = cvm.Name;
                    if (cvm.TypeOfDocumentId == 1) condition.TypeOfStateRegId = cvm.TypeOfStateRegId;
                    condition.DocumentTemplateId = cvm.DocumentTemplateId;
                    condition.NumLevelReference = cvm.NumLevelReference;
                    condition.Justification = cvm.Justification;
                    int numberOfConditions = await GetNumberOfConditionsInDocumentTemplate(condition.DocumentTemplateId);
                    condition.NumberInDocument = numberOfConditions + 1;

                db.Conditions.Add(condition);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать пункт шаблона", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditCondition(ConditionViewModel cvm)
        {
            try
            {
                Condition condition = await db.Conditions.FirstOrDefaultAsync(p => p.Id == cvm.Id);
                condition.Title = cvm.Title;
                condition.Name = cvm.Name;
                if (cvm.TypeOfDocumentId == 1)  condition.TypeOfStateRegId = cvm.TypeOfStateRegId;
                condition.DocumentTemplateId = cvm.DocumentTemplateId;
                condition.NumLevelReference = cvm.NumLevelReference;
                condition.NumId = cvm.NumId;
                condition.Justification = cvm.Justification;
                db.Conditions.Update(condition);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось открыть пункт шаблона для редактирования", HttpStatusCode.BadRequest);
            }
        }

        public async Task SaveCondition(Condition condition)
        {
            try
            {
                db.Conditions.Update(condition);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось сохранить пункт шаблона", HttpStatusCode.BadRequest);
            }
        }

        public async Task CreateSubCondition(SubConditionViewModel scvm)
        {
            try
            {
                SubCondition subCondition = new SubCondition
                {
                Name = scvm.Name,
                Text = scvm.Text,
                ConditionId = scvm.ConditionId,
                NumLevelReference = scvm.NumLevelReference,
                NumId = scvm.NumId,
                Justification = scvm.Justification
                };

            db.SubConditions.Add(subCondition);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать подпункт шаблона", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditSubCondition(SubConditionViewModel scvm)
        {
            try
            {
                SubCondition subCondition = await db.SubConditions.FirstOrDefaultAsync(p => p.Id == scvm.Id);
                subCondition.Name = scvm.Name;
                subCondition.Text = scvm.Text;
                subCondition.ConditionId = scvm.ConditionId;
                subCondition.NumLevelReference = scvm.NumLevelReference;
                subCondition.NumId = scvm.NumId;
                subCondition.Justification = scvm.Justification;

                db.SubConditions.Update(subCondition);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось открыть подпункт шаблона для редактирования", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteSubCondition(int id)
        {
            try
            {
                SubCondition subCondition = await db.SubConditions.FirstOrDefaultAsync(p => p.Id == id);
                db.SubConditions.Remove(subCondition);
                await db.SaveChangesAsync();
            }
            catch 
            {
                throw new AisException("Не удалось удалить подпункт шаблона", HttpStatusCode.BadRequest);
            }
        }

        public async Task CreateSubConditionParagraph(SubConditionParagraphViewModel scpvm)
        {
            try
            {
                SubConditionParagraph subConditionParagraph = new SubConditionParagraph
                {
                    Text = scpvm.Text,
                    SubConditionId = scpvm.SubConditionId,
                    NumLevelReference = scpvm.NumLevelReference,
                    NumId = scpvm.NumId,
                    Justification = scpvm.Justification
                };

                db.SubConditionParagraphs.Add(subConditionParagraph);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать абзац", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditSubConditionParagraph(SubConditionParagraphViewModel scpvm)
        {
            try
            {
                SubConditionParagraph subConditionParagraph = await db.SubConditionParagraphs.FirstOrDefaultAsync(p => p.Id == scpvm.Id);
                subConditionParagraph.Text = scpvm.Text;
                subConditionParagraph.SubConditionId = scpvm.SubConditionId;
                subConditionParagraph.NumLevelReference = scpvm.NumLevelReference;
                subConditionParagraph.NumId = scpvm.NumId;
                subConditionParagraph.Justification = scpvm.Justification;

                db.SubConditionParagraphs.Update(subConditionParagraph);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось открыть абзац для редактирования", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteSubConditionParagraph(int id)
        {
            try { 
                SubConditionParagraph subConditionParagraph = await db.SubConditionParagraphs.FirstOrDefaultAsync(p => p.Id == id);
                db.SubConditionParagraphs.Remove(subConditionParagraph);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось удалить абзац", HttpStatusCode.BadRequest);
            }
        }

        public async Task<DocumentTemplate> GetDocumentTemplateById(int id)
        {

                DocumentTemplate? documentTemplate = await db.DocumentTemplates
                    .FirstOrDefaultAsync(p => p.Id == id);
                if(documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
                return documentTemplate;
        }

        public async Task<DocumentTemplate> GetDocumentTemplateWithConditionsById(int id)
        {
                DocumentTemplate? documentTemplate = await db.DocumentTemplates
                    .Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if(documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
                return documentTemplate;
        }

        public async Task<DocumentTemplate> GetDocumentTemplateWithRootTemplateById(int id)
        {
                DocumentTemplate? documentTemplate = await db.DocumentTemplates
                    .Include(p => p.RootTemplate)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
                return documentTemplate;
        }

        public async Task<DocumentTemplate> GetDocumentTemplateWithTypeOfContractById(int id)
        {
                DocumentTemplate? documentTemplate = await db.DocumentTemplates
                    .Include(p => p.TypeOfContract)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
                return documentTemplate;
       }

        public async Task DeleteDocumentTemplate(int id)
        {
            try
            {
                DocumentTemplate DocumentTemplate = await db.DocumentTemplates.FirstOrDefaultAsync(p => p.Id == id);
                db.DocumentTemplates.Remove(DocumentTemplate);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось удалить шаблон документа", HttpStatusCode.BadRequest);
            }
        }


        public async Task<List<Condition>> GetConditions()
        {
            try
            {
                return await db.Conditions.Include(p => p.SubConditions).ToListAsync();
            }
            catch
            {
                throw new AisException("Не удалось получить список пунктов для шаблонов документов", HttpStatusCode.BadRequest);
            }
        }

        public async Task<int> GetNumberOfConditionsInDocumentTemplate(int id)
        {
             DocumentTemplate? documentTemplate = await db.DocumentTemplates.Include(p => p.Conditions).FirstOrDefaultAsync(p => p.Id == id);
            if (documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
            return documentTemplate.Conditions.Count();
        }

        public async Task<Condition> GetCondition(int id)
        {
                Condition? condition = await db.Conditions
                    .Include(p=>p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if(condition == null) throw new AisException("Не удалось получить пункт шаблона", HttpStatusCode.BadRequest);
                return condition;
        }

        public async Task<Condition> GetConditionByNumberInDocument(int number)
        {

                Condition? condition = await db.Conditions
                    .FirstOrDefaultAsync(p => p.NumberInDocument == number);
                if (condition == null) throw new AisException("Не удалось получить пункт шаблона", HttpStatusCode.BadRequest);
                return condition;
        }

        public async Task<SubCondition> GetSubCondition(int id)
        {
                SubCondition? subCondition = await db.SubConditions
                    .Include(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (subCondition == null) throw new AisException("Не удалось получить подпункт шаблона", HttpStatusCode.BadRequest);
                return subCondition;
        }

        public async Task<SubConditionParagraph> GetSubConditionParagraph(int id)
        {
                SubConditionParagraph? subConditionParagraph = await db.SubConditionParagraphs
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (subConditionParagraph == null) throw new AisException("Не удалось получить абзац", HttpStatusCode.BadRequest);
                return subConditionParagraph;
        }

        public async Task DeleteCondition(int id)
        {
            try
            {
                Condition condition = await db.Conditions.FirstOrDefaultAsync(p => p.Id == id);
                db.Conditions.Remove(condition);
                await db.SaveChangesAsync();
            }
            catch 
            {
                throw new AisException("Не удалось удалить пункт", HttpStatusCode.BadRequest);
            }
        }

    }
}

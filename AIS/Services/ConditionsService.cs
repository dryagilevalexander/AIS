using AIS.ErrorManager;
using AIS.ViewModels.DocumentsViewModels;
using Infrastructure;
using Infrastructure.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIS.ErrorManager;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.Services
{
    public class ConditionsService : IConditionsService
    {
        private AisDbContext db;

        public ConditionsService(AisDbContext context)
        {
            db = context;
        }

        public async Task<List<RootTemplate>> GetRootTemplates()
        {
            try
            {
                return await db.RootTemplates.ToListAsync();
            }
            catch
            {
                throw new AisException("Не удалось получить корневые шаблоны документов", HttpStatusCode.BadRequest);
            }
        }

        public async Task<RootTemplate> GetRootTemplateById(int id)
        {
            RootTemplate? rootTemplate = await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == id);
            if (rootTemplate == null) throw new AisException("Не найден корневой шаблон документа", HttpStatusCode.BadRequest);
            return rootTemplate;
        }

        public async Task<RootTemplate> GetRootTemplateWithDocumentTemplatesById(int id)
        {
             RootTemplate? rootTemplate = await db.RootTemplates.Include(p => p.DocumentTemplates).FirstOrDefaultAsync(p => p.Id == id);
             if (rootTemplate == null) throw new AisException("Не найден корневой шаблон документа", HttpStatusCode.BadRequest);
             return rootTemplate;
        }

        public async Task<List<DocumentTemplate>> GetDocumentTemplates()
        {
            try 
            { 
            return await db.DocumentTemplates.ToListAsync();
            }
            catch
            {
                throw new AisException("Не удалось получить шаблоны документов", HttpStatusCode.BadRequest);
            }
        }

        public async Task<List<DocumentTemplate>> GetDocumentTemplatesByTypeOfDocumentId(int id)
        {
            try
            {
                return await db.DocumentTemplates.Where(p => p.TypeOfDocumentId == 1).ToListAsync();
            }
            catch
            {
                throw new AisException("Не удалось получить шаблоны документов", HttpStatusCode.BadRequest);
            }
        }

        //Метод получения шаблона контракта с стандартными условиями для всех типов регулирования
        public DocumentTemplate GetDocumentTemplateEagerLoadingById(int id)
        {
            DocumentTemplate? documentTemplate = db.DocumentTemplates.Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(c => c.SubConditionParagraphs).FirstOrDefault(p => p.Id == id);
            if(documentTemplate == null) throw new AisException("Не найден шаблон документа", HttpStatusCode.BadRequest);
            return documentTemplate;
        }

        public async Task CreateRootTemplate(CreateRootTemplateViewModel model)
        {
            try
            {
                RootTemplate RootTemplate = new RootTemplate();

                RootTemplate.Name = model.Name;
                RootTemplate.Description = model.Description;
                RootTemplate.TypeOfDocumentId = model.TypeOfDocumentId;

                db.RootTemplates.Add(RootTemplate);
                await db.SaveChangesAsync();
            }

            catch
            {
                throw new AisException("Не удалось создать шаблон", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditRootTemplate(EditRootTemplateViewModel model)
        {
            try
            {
                RootTemplate? rootTemplate = await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == model.Id);
                if(rootTemplate == null) throw new AisException("Не найден корневой шаблон документа", HttpStatusCode.BadRequest);
                rootTemplate.Name = model.Name;
                rootTemplate.Description = model.Description;
                rootTemplate.TypeOfDocumentId = model.TypeOfDocumentId;

                db.RootTemplates.Update(rootTemplate);
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


        public async Task CreateDocumentTemplate(CreateDocumentTemplateViewModel model)
        {

            try
            {
                DocumentTemplate documentTemplate = new DocumentTemplate
                {
                    Name = model.Name,
                    Description = model.Description,
                    RootTemplateId = model.RootTemplateId,
                    TypeOfDocumentId = model.TypeOfDocumentId
                };
                if (model.TypeOfContractId != null) documentTemplate.TypeOfContractId = model.TypeOfContractId;

                db.DocumentTemplates.Add(documentTemplate);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать шаблон", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditDocumentTemplate(EditDocumentTemplateViewModel model)
        {
            try
            {
                DocumentTemplate DocumentTemplate = await db.DocumentTemplates.FirstOrDefaultAsync(p => p.Id == model.Id);
                DocumentTemplate.Name = model.Name;
                DocumentTemplate.Description = model.Description;
                DocumentTemplate.TypeOfContractId = model.TypeOfContractId;

                db.DocumentTemplates.Update(DocumentTemplate);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось открыть шаблон для редактирования", HttpStatusCode.BadRequest);
            }
        }

        public async Task CreateCondition(CreateConditionViewModel model)
        {
            try
            {
                Condition condition = new Condition();

                condition.Title = model.Title;
                condition.Name = model.Name;
                if (model.TypeOfDocumentId == 1) condition.TypeOfStateRegId = model.TypeOfStateRegId;
                condition.DocumentTemplateId = model.DocumentTemplateId;
                condition.NumLevelReference = model.NumLevelReference;
                condition.Justification = model.Justification;
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

        public async Task EditCondition(EditConditionViewModel model)
        {
            try
            {
                Condition condition = await db.Conditions.FirstOrDefaultAsync(p => p.Id == model.Id);
                condition.Title = model.Title;
                condition.Name = model.Name;
                if (model.TypeOfDocumentId == 1) condition.TypeOfStateRegId = model.TypeOfStateRegId;
                condition.DocumentTemplateId = model.DocumentTemplateId;
                condition.NumLevelReference = model.NumLevelReference;
                condition.NumId = model.NumId;
                condition.Justification = model.Justification;
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

        public async Task CreateSubCondition(CreateSubConditionViewModel model)
        {
            try
            {
                SubCondition subCondition = new SubCondition
                {
                    Name = model.Name,
                    Text = model.Text,
                    ConditionId = model.ConditionId,
                    NumLevelReference = model.NumLevelReference,
                    NumId = model.NumId,
                    Justification = model.Justification
                };

                db.SubConditions.Add(subCondition);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать подпункт шаблона", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditSubCondition(EditSubConditionViewModel model)
        {
            try
            {
                SubCondition? subCondition = await db.SubConditions.FirstOrDefaultAsync(p => p.Id == model.Id);
                if(subCondition == null) throw new AisException("Не удалось найти подпункт", HttpStatusCode.BadRequest);
                subCondition.Name = model.Name;
                subCondition.Text = model.Text;
                subCondition.ConditionId = model.ConditionId;
                subCondition.NumLevelReference = model.NumLevelReference;
                subCondition.NumId = model.NumId;
                subCondition.Justification = model.Justification;

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

        public async Task CreateSubConditionParagraph(CreateSubConditionParagraphViewModel model)
        {
            try
            {
                SubConditionParagraph subConditionParagraph = new SubConditionParagraph
                {
                    Text = model.Text,
                    SubConditionId = model.SubConditionId,
                    NumLevelReference = model.NumLevelReference,
                    NumId = model.NumId,
                    Justification = model.Justification
                };

                db.SubConditionParagraphs.Add(subConditionParagraph);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать абзац", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditSubConditionParagraph(EditSubConditionParagraphViewModel model)
        {
            try
            {
                SubConditionParagraph? subConditionParagraph = await db.SubConditionParagraphs.FirstOrDefaultAsync(p => p.Id == model.Id);
                if(subConditionParagraph == null) throw new AisException("Не найден абзац", HttpStatusCode.BadRequest);
                subConditionParagraph.Text = model.Text;
                subConditionParagraph.SubConditionId = model.SubConditionId;
                subConditionParagraph.NumLevelReference = model.NumLevelReference;
                subConditionParagraph.NumId = model.NumId;
                subConditionParagraph.Justification = model.Justification;

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
            try
            {
                SubConditionParagraph? subConditionParagraph = await db.SubConditionParagraphs.FirstOrDefaultAsync(p => p.Id == id);
                if (subConditionParagraph == null) throw new AisException("Не найден абзац", HttpStatusCode.BadRequest);
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
            if (documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
            return documentTemplate;
        }

        public async Task<DocumentTemplate> GetDocumentTemplateWithConditionsById(int id)
        {
            DocumentTemplate? documentTemplate = await db.DocumentTemplates
                .Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (documentTemplate == null) throw new AisException("Не обнаружен шаблон документа", HttpStatusCode.BadRequest);
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
                .Include(p => p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (condition == null) throw new AisException("Не удалось получить пункт шаблона", HttpStatusCode.BadRequest);
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
                Condition? condition = await db.Conditions.FirstOrDefaultAsync(p => p.Id == id);
                if(condition == null) throw new AisException("Не найден пункт шаблона", HttpStatusCode.BadRequest);
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
